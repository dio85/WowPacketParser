using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.Substructures
{
    public static class MythicPlusHandler
    {
        public static void ReadDungeonScoreMapSummary(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("ChallengeModeID", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_5_40772))
                packet.ReadSingle("MapScore", indexes);
            else
                packet.ReadInt32("MapScore", indexes);

            packet.ReadInt32("BestRunLevel", indexes);
            packet.ReadInt32("BestRunDurationMS", indexes);

            packet.ResetBitReader();
            packet.ReadBit("FinishedSuccess", indexes);
        }

        public static void ReadDungeonScoreSummary(Packet packet, params object[] indexes)
        {
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_5_40772))
                packet.ReadSingle("OverallScoreCurrentSeason", indexes);
            else
                packet.ReadInt32("OverallScoreCurrentSeason", indexes);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_2_0_42423))
                packet.ReadSingle("LadderScoreCurrentSeason", indexes);

            var runCount = packet.ReadUInt32("RunCount", indexes);
            for (var i = 0u; i < runCount; ++i)
                ReadDungeonScoreMapSummary(packet, indexes, i, "Run");
        }

        public static void ReadMythicPlusMember(Packet packet, params object[] indexes)
        {
            packet.ReadPackedGuid128("BnetAccountGUID", indexes);
            packet.ReadUInt64("GuildClubMemberID", indexes);
            packet.ReadPackedGuid128("GUID", indexes);
            packet.ReadPackedGuid128("GuildGUID", indexes);
            packet.ReadUInt32("NativeRealmAddress", indexes);
            packet.ReadUInt32("VirtualRealmAddress", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_2_0_42423))
                packet.ReadInt32("ChrSpecializationID", indexes);
            else
                packet.ReadInt16("ChrSpecializationID", indexes);

            packet.ReadInt16E<Race>("RaceID", indexes);
            packet.ReadInt32("ItemLevel", indexes);
            packet.ReadInt32("CovenantID", indexes);
            packet.ReadInt32("SoulbindID", indexes);
        }

        public static void ReadMythicPlusRun(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("MapChallengeModeID", indexes);
            packet.ReadUInt32("Level", indexes);
            packet.ReadInt32("DurationMs", indexes);
            packet.ReadTime64("StartDate", indexes);
            packet.ReadTime64("CompletionDate", indexes);
            packet.ReadInt32("Season", indexes);
            for (var i = 0; i < 4; ++i)
                packet.ReadUInt32("KeystoneAffixIDs", indexes, i);

            var memberCount = packet.ReadUInt32("MemberCount", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_5_40772))
                packet.ReadSingle("RunScore", indexes);
            else
                packet.ReadInt32("RunScore", indexes);

            for (var i = 0u; i < memberCount; ++i)
                ReadMythicPlusMember(packet, indexes, i, "Member");

            packet.ResetBitReader();
            packet.ReadBit("Completed", indexes);
        }

        public static void ReadDungeonScoreBestRunForAffix(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("KeystoneAffixID", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_5_40772))
                packet.ReadSingle("Score", indexes);
            else
                packet.ReadInt32("Score", indexes);

            ReadMythicPlusRun(packet, indexes, "Run");
        }

        public static void ReadDungeonScoreMapData(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("MapChallengeModeID", indexes);
            var runCount = packet.ReadUInt32("BestRunCount", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_5_40772))
                packet.ReadSingle("OverAllScore", indexes);
            else
                packet.ReadInt32("OverAllScore", indexes);

            for (var i = 0u; i < runCount; ++i)
                ReadDungeonScoreBestRunForAffix(packet, indexes, i, "BestRun");
        }

        public static void ReadDungeonScoreSeasonData(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("Season", indexes);
            var runCount = packet.ReadUInt32("MapCount", indexes);
            var runCount2 = 0u;
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_2_0_42423))
                runCount2 = packet.ReadUInt32("LadderMapCount", indexes);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_1_5_40772))
                packet.ReadSingle("SeasonScore", indexes);
            else
                packet.ReadInt32("SeasonScore", indexes);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V9_2_0_42423))
                packet.ReadSingle("LadderScore", indexes);

            for (var i = 0u; i < runCount; ++i)
                ReadDungeonScoreMapData(packet, indexes, i, "Map");

            for (var i = 0u; i < runCount2; ++i)
                ReadDungeonScoreMapData(packet, indexes, i, "LadderMap");
        }

        public static void ReadDungeonScoreData(Packet packet, params object[] indexes)
        {
            var seasonCount = packet.ReadUInt32("SeasonCount", indexes);
            packet.ReadInt32("TotalRuns", indexes);
            for (var i = 0u; i < seasonCount; ++i)
                ReadDungeonScoreSeasonData(packet, indexes, i, "Season");
        }

        [Parser(Opcode.CMSG_MYTHIC_PLUS_REQUEST_MAP_STATS)]
        public static void HandleMythicPlusRequestMapStats(Packet packet)
        {
            packet.ReadPackedGuid128("BnetAccountGUID");
            packet.ReadInt64("MapChallengeModeID");

        }

        [Parser(Opcode.SMSG_CHALLENGE_MODE_START)]
        public static void HandleChallengeModeStart(Packet packet)
        {
            packet.ReadInt32("MapID");
            packet.ReadInt32("ChallengeID");
            packet.ReadInt32("ChallengeLevel");

            for (int i = 0; i < 4; i++)
                packet.ReadInt32("Affixes", i);

            packet.ReadInt32("DeathCount");
            var count = packet.ReadInt32("ClientEncounterStartPlayerInfo");

            packet.ResetBitReader();
            packet.ReadBit("Energized");

            //for (int i = 0; i < count; i++)
            //   ReadUnkStruct1(packet, "ReadUnkStruct1", i); need make the unk struct
        }


        [Parser(Opcode.SMSG_MYTHIC_PLUS_ALL_MAP_STATS)]

        public static void HandleMythicPlusAllMapStats(Packet packet)
        {
            var runCount =packet.ReadInt32("RunCount");
            var runCount2 = 0u;
            var rewardCount = packet.ReadInt32("RewardCount");
            packet.ReadInt32("Season");
            packet.ReadInt32("SubSeason");

            for (var i = 0u; i < runCount; i++)
                ReadDungeonScoreSeasonData(packet, i, "Map");

            for (var i = 0u; i < rewardCount; i++)
                packet.ReadInt32("RewardCount");
        }
    }
}
