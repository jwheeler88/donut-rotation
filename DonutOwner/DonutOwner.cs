using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Donuts
{
    public class DonutRotator
    {
        private string _rotationXmlPath;
        private CircularQueue _cQueue;
        
        public DonutRotator(string rotationXmlPath)
        {
            _rotationXmlPath = rotationXmlPath;
        }

        public List<string> ConstructRotationDetails()
        {
            DeSerialize();
            var rotationInfo = CreateRotationInfo();
            Serialize();

            return rotationInfo;
        }

        private void Serialize()
        {
            var writer = new XmlSerializer(typeof(List<DonutOwner>));
            var wfile = new StreamWriter(_rotationXmlPath);
            writer.Serialize(wfile, _cQueue.GetQueue());
            wfile.Close();
        }

        private void DeSerialize()
        {
            XmlSerializer reader = new XmlSerializer(typeof(List<DonutOwner>));
            StreamReader file = new StreamReader(_rotationXmlPath);

            var owners = (List<DonutOwner>)reader.Deserialize(file);
            file.Close();

            _cQueue = new CircularQueue(owners.Count);
            owners.ForEach(o => _cQueue.Add(o));
        }

        private DonutOwner ProcessQueue()
        {
            var owner = _cQueue.Read();
            _cQueue.Add(owner);

            return owner;
        }

        private List<string> CreateRotationInfo()
        {
            var todaysOwner = ProcessQueue();
            var nextOwner = _cQueue.Peek(); //Queue has been processed, so next person is first up in line.

            var rotationInfo = new List<string>();

            rotationInfo.Add(todaysOwner.name);
            rotationInfo.Add(nextOwner.name);

            string ownerOrder = string.Empty;
            string ownerEmails = string.Empty;
            foreach (var owner in _cQueue.GetQueue())
            {
                ownerOrder += owner.name + "<br>";
                ownerEmails += owner.emailAddress + ";";
            }
            ownerEmails = ownerEmails.TrimEnd(';');

            rotationInfo.Add(ownerOrder);
            rotationInfo.Add(ownerEmails);

            return rotationInfo;
        }
    }

    public class DonutOwner
    {
        public string name;
        public string emailAddress;

        public DonutOwner()
        {
            //For XML Serialization
        }

        public DonutOwner(string name, string emailAddress)
        {
            this.name = name;
            this.emailAddress = emailAddress;
        }
    }

    public class CircularQueue
    {
        private int _size;
        private Queue<DonutOwner> _queue;

        public CircularQueue(int size)
        {
            _queue = new Queue<DonutOwner>(size);
            _size = size;
        }

        public List<DonutOwner> GetQueue()
        {
            return _queue.ToList();
        }

        public void Add(DonutOwner owner)
        {
            if (_queue.Count == _size)
            {
                _queue.Dequeue();
                _queue.Enqueue(owner);
            }
            else
            {
                _queue.Enqueue(owner);
            }
        }

        public DonutOwner Peek()
        {
            return _queue.Peek();
        }

        public DonutOwner Read()
        {
            return _queue.Dequeue();
        }
    }
}
