using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [Hotfix]
    [DBTableName("garr_class_spec")]
    public sealed record GarrClassSpecHotfix440: IDataModel
    {
        [DBFieldName("ClassSpec")]
        public string ClassSpec;

        [DBFieldName("ClassSpecMale")]
        public string ClassSpecMale;

        [DBFieldName("ClassSpecFemale")]
        public string ClassSpecFemale;

        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("UiTextureAtlasMemberID")]
        public ushort? UiTextureAtlasMemberID;

        [DBFieldName("GarrFollItemSetID")]
        public ushort? GarrFollItemSetID;

        [DBFieldName("FollowerClassLimit")]
        public byte? FollowerClassLimit;

        [DBFieldName("Flags")]
        public byte? Flags;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }

    [Hotfix]
    [DBTableName("garr_class_spec_locale")]
    public sealed record GarrClassSpecLocaleHotfix440: IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("locale", true)]
        public string Locale = ClientLocale.PacketLocaleString;

        [DBFieldName("ClassSpec_lang")]
        public string ClassSpecLang;

        [DBFieldName("ClassSpecMale_lang")]
        public string ClassSpecMaleLang;

        [DBFieldName("ClassSpecFemale_lang")]
        public string ClassSpecFemaleLang;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
