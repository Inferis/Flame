using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Inferis.Apis.Campfire
{
    public class CampfireMessageReader : IDisposable
    {
        private StreamReader reader;

        public CampfireMessageReader(Stream stream)
        {
            reader = new StreamReader(stream);
        }

        public Stream BaseStream { get { return reader.BaseStream; } }

        public void Dispose()
        {
            Close();
        }

        public bool Peek()
        {
            while (reader.Peek() == 32)
                reader.Read();

            return reader.Peek() > 0;
        }

        public CampfireMessage Read()
        {
            var chars = new List<char>();
            var last = '\0';
            while (last != '}') {
                last = (char)reader.Read();
                chars.Add(last);
            }
            return Parse(new string(chars.ToArray()));
        }

        private CampfireMessage Parse(string line)
        {
            return new CampfireMessage();
			/*
			{
                RoomId = Convert.ToInt32(converted.room_id ?? "0"),
                Id = Convert.ToInt32(converted.id ?? "0"),
                UserId = Convert.ToInt32(converted.user_id ?? "0"),
                CreatedAt = Convert.ToDateTime(converted.created_at),
                Type = (CampfireMessageType)Enum.Parse(typeof(CampfireMessageType), converted.type),
                Body = converted.body
            };
            */
        }

        public void Close()
        {
            var r = reader;
            reader = null;
            if (r == null) return;
            r.Close();
            r.Dispose();
        }
    }
}
