using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Model;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.Service {
    public class MostRecentlyUsedFileService : IMostRecentlyUsedFileService {
        private readonly string _mostRecentlyUsedFilePath;
        private readonly IEventAggregator _eventAggregator;
        private readonly IXmlSerializerService _xmlSerializerService;

        public MostRecentlyUsedFileService(IEventAggregator eventAggregator, IXmlSerializerService xmlSerializerService) {
            this._mostRecentlyUsedFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "MostRecentlyUsed.xml");
            this._eventAggregator = eventAggregator;
            this._xmlSerializerService = xmlSerializerService;
        }

        public void AddFile(string fileName) {
            List<MostRecentlyUsed> list = this.LoadList();

            list.Add(new MostRecentlyUsed {
                FileName = fileName,
                LastUsed = DateTime.Now
            });

            list = this.ReduceList(list);

            File.WriteAllText(this._mostRecentlyUsedFilePath, this._xmlSerializerService.ToXml(list), Encoding.Unicode);
            this._eventAggregator.GetEvent<MostRecentlyUsedFileChangedEvent>().Publish(list);
        }

        private List<MostRecentlyUsed> ReduceList(List<MostRecentlyUsed> list) {
            return (from m in
                        (from d in list
                         group d by d.FileName.ToLower() into g
                         select new MostRecentlyUsed {
                             FileName = g.Key,
                             LastUsed = g.Max(item => item.LastUsed)
                         })
                    orderby m.LastUsed descending
                    select m).Take(10).ToList();
        }

        public ReadOnlyCollection<MostRecentlyUsed> GetList() {
            return this.LoadList().AsReadOnly();
        }

        private List<MostRecentlyUsed> LoadList() {
            if (File.Exists(this._mostRecentlyUsedFilePath)) {
                return this._xmlSerializerService.DeserializeFile<List<MostRecentlyUsed>>(this._mostRecentlyUsedFilePath)
                                                 .OrderByDescending(d => d.LastUsed)
                                                 .ToList();
            }

            return new List<MostRecentlyUsed>();
        }
    }
}
