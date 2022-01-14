using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CustomNameList
{
    class CustomNameService {
        private static Random rng;
        private readonly string _textFile;
        private List<String> _allNames;
        private Stack<String> _nextNames;


        internal bool IsInitialized { get; private set; }

        public CustomNameService(string textFile) {
            _textFile = textFile;
        }

        internal void Init() {
            if (!File.Exists(_textFile)) {
                Plugin.Log.LogError($"Could not find names file at: {_textFile}");
                Plugin.Log.LogWarning($"Will use standard game names instead.");
                return;
            }

            _allNames = File.ReadAllLines(_textFile).ToList().Select(e => e.Trim().Replace("\r", "")).ToList();

            Plugin.Log.LogInfo($"Read {_allNames.Count()} names from {_textFile}");

            LoadNames();

            IsInitialized = true;
        }

        internal String NextName() {
            if(_nextNames.Count == 0)
                RefillNames();

            var nextName = _nextNames.Pop();
            Plugin.NameServiceLastIndex.Value = Plugin.NameServiceLastIndex.Value + 1;
            Console.WriteLine(string.Join(", ", _nextNames));
            return nextName;
        }

        private void LoadNames()
        {
            if (Plugin.NameServiceLastIndex.Value > _allNames.Count)
                RefillNames();
            int curTick = Plugin.NameServiceRandomSeed.Value;
            rng = new Random(curTick);
            Console.WriteLine(Plugin.NameServiceRandomSeed.Value);
            Console.WriteLine(Plugin.NameServiceLastIndex.Value);
            List<String> reordered = _allNames.OrderBy(a => rng.Next()).ToList();
            Console.WriteLine(string.Join(", ", _allNames));
            Console.WriteLine(string.Join(", ", reordered));
            Console.WriteLine(string.Join(", ", reordered.SkipLast(Plugin.NameServiceLastIndex.Value)));

            _nextNames = new Stack<String>(reordered.SkipLast(Plugin.NameServiceLastIndex.Value));
            Console.WriteLine(string.Join(", ", _nextNames));
        }
        private void RefillNames()
        {
            Console.WriteLine("Refilling Names");
            int curTick = Environment.TickCount;
            rng = new Random(curTick);
            Plugin.NameServiceRandomSeed.Value = curTick;
            Plugin.NameServiceLastIndex.Value = 0;
            _nextNames = new Stack<String>(_allNames.OrderBy(a => rng.Next()).ToList());
        }
    }
}