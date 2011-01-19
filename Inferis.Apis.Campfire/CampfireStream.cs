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
            ThreadPool.QueueUserWorkItem(o => {
                while (streaming) {
                    try {
                        var uri = string.Format("https://streaming.campfirenow.com/room/{0}/live.xml", Room);
                        var request = (HttpWebRequest)WebRequest.Create(uri);
                        request.KeepAlive = true;
                        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(token + ":x")));

                        var response = (HttpWebResponse)request.GetResponse();
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
                    catch (WebException) {
                    }
                    catch (TimeoutException) {
                    }
                }
            });
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
