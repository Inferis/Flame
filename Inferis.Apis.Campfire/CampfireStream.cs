using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;

namespace Inferis.Apis.Campfire
{
    public class CampfireStream : IDisposable
    {
        private Queue<CampfireMessage> queue = new Queue<CampfireMessage>();
        private readonly object queueLock = new object();
        private readonly string token;
        private bool streaming;

        public CampfireStream(string room, string token)
        {
            this.token = token;
            Room = room;
            StartStream();
        }

        private void StartStream()
        {
            streaming = true;
            try {
                var uri = string.Format("https://streaming.campfirenow.com/room/{0}/live.xml", Room);
                var request = (HttpWebRequest)WebRequest.Create(uri);
#if !SILVERLIGHT
                request.KeepAlive = true;
#endif
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(token + ":x"));

                request.BeginGetResponse(EndResponse, request);
            }
            catch (WebException) {
            }
            catch (TimeoutException) {
            }
        }

        private void EndResponse(IAsyncResult ar)
        {
            var request = (HttpWebRequest) ar.AsyncState;
            var response = request.EndGetResponse(ar);
            var stream = response.GetResponseStream();
            var reader = new CampfireMessageReader(stream);
            try {
                while (streaming) {
                    OnMessageReceived(new MessageReceivedEventArgs(reader.Read()));
                    Thread.Sleep(100);
                }
            }
            finally {
                reader.Close();
                stream.Close();
            }
        }

        public event MessageReceivedEventHandler MessageReceived;

        protected virtual void OnMessageReceived(MessageReceivedEventArgs args)
        {
            var handler = MessageReceived;
            if (handler != null) handler(this, args);
        }

        protected string Room { get; private set; }

        public void Close()
        {
            streaming = false;
        }

        public void Dispose()
        {
            Close();
        }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs args);

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(CampfireMessage message)
        {
            Message = message;
        }

        public CampfireMessage Message { get; private set; }
    }
}
