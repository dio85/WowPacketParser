// <auto-generated>
// DO NOT EDIT
// </auto-generated>

using System.CodeDom.Compiler;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects.UpdateFields;

namespace WowPacketParserModule.V4_4_0_54481.UpdateFields.V4_4_1_57294
{
    [GeneratedCode("UpdateFieldCodeGenerator.Formats.WowPacketParserHandler", "1.0.0.0")]
    public class QuestLog : IQuestLog
    {
        public System.Nullable<long> EndTime { get; set; }
        public System.Nullable<int> QuestID { get; set; }
        public System.Nullable<uint> StateFlags { get; set; }
        public System.Nullable<ushort>[] ObjectiveProgress { get; } = new System.Nullable<ushort>[24];
    }
}

