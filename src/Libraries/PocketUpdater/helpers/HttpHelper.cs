using System.IO;
using System.Net.Http;

namespace pannella.analoguepocket;

public class HttpHelper
{
    private static HttpHelper instance = null;
    private static object syncLock = new object();
    private HttpClient client = null;
    public event EventHandler<DownloadProgressEventArgs> DownloadProgressUpdate;

    private HttpHelper()
    {
        createClient();
    }

    public static HttpHelper Instance
    {
        get
        {
            lock (syncLock)
            {
                if (HttpHelper.instance == null) {
                    HttpHelper.instance = new HttpHelper();
                }

                return HttpHelper.instance;
            }
        }
    }


   public async Task DownloadFileAsync(string uri, string outputPath, int timeout = 100)
   {
        bool console = false;
        try {
            var test = Console.WindowWidth;
            console = true;
        } catch (Exception) { }

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));
        Uri? uriResult;

        if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            throw new InvalidOperationException("URI is invalid.");

        using HttpResponseMessage r = await this.client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cts.Token);

        var totalSize = r.Content.Headers.ContentLength ?? -1L;
        var readSoFar = 0L;
        var buffer = new byte[4096];
        var isMoreToRead = true;

        using var stream = await r.Content.ReadAsStreamAsync();
        using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

        while (isMoreToRead)
        {
            var read = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
            {
                isMoreToRead = false;
                if (console) {
                    Console.Write("\r");
                }
            }
            else
            {
                readSoFar += read;
                var progress = (double)readSoFar / totalSize;
                if (console) {
                    var progressWidth = Console.WindowWidth - 14;
                    var progressBarWidth = (int)(progress * progressWidth);
                    var progressBar = new string('=', progressBarWidth);
                    var emptyProgressBar = new string(' ', progressWidth - progressBarWidth);
                    Console.Write($"\r{progressBar}{emptyProgressBar}] {(progress * 100):0.00}%");
                    if (readSoFar == totalSize)
                    {
                        Console.CursorLeft = 0;
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.CursorLeft = 0;
                    }
                }
                DownloadProgressEventArgs args = new DownloadProgressEventArgs();
                args.progress = progress;
                OnDownloadProgressUpdate(args);
                await fileStream.WriteAsync(buffer, 0, read);
            }
        }
    }

   public async Task<String> GetHTML(string uri, bool allowRedirect = true)
   {
        Uri? uriResult;

        if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            throw new InvalidOperationException("URI is invalid.");

        if(!allowRedirect) {
            createClient(false);
        }

        var response = await this.client.GetAsync(uri);
        string html = await response.Content.ReadAsStringAsync();

        if(!allowRedirect) {
            createClient();
        }
        
        return html;
   }

   private void createClient(bool allowRedirect = true)
   {
        this.client = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = allowRedirect });
        this.client.Timeout = TimeSpan.FromMinutes(10); //10min
   }

   protected virtual void OnDownloadProgressUpdate(DownloadProgressEventArgs e)
    {
        EventHandler<DownloadProgressEventArgs> handler = DownloadProgressUpdate;
        if(handler != null)
        {
            handler(this, e);
        }
    }
}

public class DownloadProgressEventArgs : EventArgs
{
    public double progress = 0;
}
