using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V10_0_0_46181.Parsers
{
    public static class CraftingHandler
    {
        public static void ReadCraftingReagentBase(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();
            var hasItem = packet.ReadBit();
            var hasCurrency = packet.ReadBit();

            if (hasItem)
                packet.ReadInt32<ItemId>("ItemID", indexes);

            if (hasCurrency)
                packet.ReadInt32<CurrencyId>("CurrencyID", indexes);
        }

        public static void ReadSpellReducedReagent(Packet packet, params object[] indexes)
        {
            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V12_0_0_65390))
                packet.ReadInt32("ItemID", indexes);

            if (ClientVersion.AddedInVersion(ClientVersionBuild.V12_1_0_69214))
                ReadCraftingReagentBase(packet, indexes, "Reagent");

            packet.ReadInt32("Quantity", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V12_0_0_65390) && ClientVersion.RemovedInVersion(ClientVersionBuild.V12_1_0_69214))
                ReadCraftingReagentBase(packet, indexes, "Reagent");
        }

        public static void ReadCraftingData(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("CraftingQualityID", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V10_0_5_47777))
                packet.ReadSingle("QualityProgress", indexes);

            packet.ReadInt32("SkillLineAbilityID", indexes);
            packet.ReadInt32("CraftingDataID", indexes);
            packet.ReadInt32("Multicraft", indexes);
            packet.ReadInt32("SkillFromReagents", indexes);
            packet.ReadInt32("Skill", indexes);
            packet.ReadInt32("CritBonusSkill", indexes);
            packet.ReadSingle("ModSkillGain", indexes);
            packet.ReadUInt64("OrderID", indexes);
            var resourcesReturnedCount = packet.ReadUInt32();
            packet.ReadUInt32("OperationID", indexes);
            packet.ReadPackedGuid128("ItemGUID", indexes);
            packet.ReadInt32("Quantity", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V12_1_0_69214))
            {
                Substructures.ItemHandler.ReadItemInstance(packet, "OldItem");
                Substructures.ItemHandler.ReadItemInstance(packet, "NewItem");
            }

            packet.ReadInt32("EnchantID", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V11_0_0_55666))
            {
                packet.ReadInt32("ConcentrationCurrencyID", indexes);
                packet.ReadInt32("ConcentrationSpent", indexes);
                packet.ReadInt32("IngenuityRefund", indexes);
            }

            for (var i = 0u; i < resourcesReturnedCount; i++)
                ReadSpellReducedReagent(packet, indexes, "ResourcesReturned", i);

            packet.ResetBitReader();
            packet.ReadBit("IsCrit", indexes);
            packet.ReadBit("IsRecraft", indexes);
            packet.ReadBit("IsInitialRecraft", indexes);
            packet.ReadBit("IsFirstCraft", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V11_0_0_55666))
            {
                packet.ReadBit("HasIngenuityProc", indexes);
                packet.ReadBit("ApplyConcentration", indexes);
            }

            if (ClientVersion.RemovedInVersion(ClientVersionBuild.V12_1_0_69214))
            {
                Substructures.ItemHandler.ReadItemInstance(packet, "OldItem");
                Substructures.ItemHandler.ReadItemInstance(packet, "NewItem");
            }
        }

        public static void ReadCraftingOrderClientContext(Packet packet, params object[] indexes)
        {
            packet.ReadByte("OrderType", indexes);
            packet.ReadUInt32("Offset", indexes);

            packet.ResetBitReader();
            packet.ReadBit("ForCrafter", indexes);
            packet.ReadBit("IsMyOrders", indexes);
            packet.ReadBit("field_3", indexes);
            packet.ReadBit("field_4", indexes);
        }

        public static void ReadCraftingOrderBucketInfo(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();
            packet.ReadBits("SkillLineAbilityID", 20, indexes);

            packet.ReadInt32("NumAvailable", indexes);
            packet.ReadUInt64("TipAmountMax", indexes);
            packet.ReadUInt64("TipAmountAvg", indexes);
        }

        public static void ReadCraftingOrderItem(Packet packet, params object[] indexes)
        {
            packet.ReadUInt64("field_0", indexes);
            packet.ReadPackedGuid128("ItemGUID", indexes);
            packet.ReadPackedGuid128("OwnerGUID", indexes);
            packet.ReadInt32<ItemId>("ItemID", indexes);
            packet.ReadUInt32("Quantity", indexes);
            packet.ReadInt32("ReagentQuality", indexes);

            packet.ResetBitReader();
            var hasDataSlotIndex = packet.ReadBit();

            if (hasDataSlotIndex)
                packet.ReadByte("DataSlotIndex", indexes);
        }

        public static void ReadCraftingOrderData(Packet packet, params object[] indexes)
        {
            packet.ReadInt32("field_0", indexes);
            packet.ReadUInt64("OrderID", indexes);
            packet.ReadInt32("SkillLineAbilityID", indexes);
            packet.ReadByte("OrderState", indexes);
            packet.ReadByte("OrderType", indexes);
            packet.ReadByte("MinQuality", indexes);
            packet.ReadTime64("ExpirationTime", indexes);
            packet.ReadTime64("ClaimEndTime", indexes);
            packet.ReadUInt64("TipAmount", indexes);
            packet.ReadUInt64("ConsortiumCut", indexes);
            packet.ReadUInt32("Flags", indexes);
            packet.ReadPackedGuid128("CustomerGUID", indexes);
            packet.ReadPackedGuid128("CustomerAccountGUID", indexes);
            packet.ReadPackedGuid128("CrafterGUID", indexes);
            packet.ReadPackedGuid128("PersonalCrafterGUID", indexes);

            var reagentsCount = packet.ReadUInt32();

            packet.ResetBitReader();
            var customerNotesLength = packet.ReadBits(10);
            var hasOutputItem = packet.ReadBit();
            var hasOutputItemData = packet.ReadBit();


            for (var i = 0u; i < reagentsCount; ++i)
                ReadCraftingOrderItem(packet, indexes, "Reagents", i);

            packet.ReadWoWString("CustomerNotes", customerNotesLength, indexes);

            if (hasOutputItem)
                ReadCraftingOrderItem(packet, indexes, "OutputItem");

            if (hasOutputItemData)
                Substructures.ItemHandler.ReadItemInstance(packet, indexes, "OutputItemInfo");
        }

        public static void ReadCraftingOrder(Packet packet, params object[] indexes)
        {
            ReadCraftingOrderData(packet, indexes, "Data");

            packet.ResetBitReader();
            var hasRecraftItemInfo = packet.ReadBit();
            var enchantmentsCount = packet.ReadBits(4);
            var gemCount = packet.ReadBits(2);


            if (hasRecraftItemInfo)
                Substructures.ItemHandler.ReadItemInstance(packet, indexes, "OutputItemInfo");

            for (var i = 0u; i < enchantmentsCount; ++i)
            {
                packet.ReadInt32("ID", indexes, "Enchantments", i);
                packet.ReadUInt32("Expiration", indexes, "Enchantments", i);
                packet.ReadInt32("Charges", indexes, "Enchantments", i);
                packet.ReadByte("Slot", indexes, "Enchantments", i);
            }

            for (var i = 0u; i < gemCount; ++i)
                Substructures.ItemHandler.ReadItemGemData(packet, indexes, "Gems", i);
        }

        public static void ReadCraftingOrderRecraftInfo(Packet packet, params object[] indexes)
        {
            // First optional string pointer + size (encoded as byte + 2 bits for value+1)
            var val1Byte = packet.ReadByte();
            var val1Bits = packet.ReadBits(2);
            var recraftItemCount = (((uint)val1Byte << 2) | (uint)val1Bits) - 1;

            // Second optional string pointer + size (encoded as byte + 2 bits for value+1)
            var val2Byte = packet.ReadByte();
            var val2Bits = packet.ReadBits(2);
            var recraftItemSlotIndex = (((uint)val2Byte << 2) | (uint)val2Bits) - 1;

            packet.AddValue("RecraftItemCount", recraftItemCount, indexes);
            packet.AddValue("RecraftItemSlotIndex", recraftItemSlotIndex, indexes);

            // Two boolean flags (at offsets +32 and +33 in the assembly)
            var isCraftingOrderRecraft = packet.ReadBit("IsCraftingOrderRecraft", indexes);
            var hasRecraftGUID = packet.ReadBit("HasRecraftGUID", indexes);
            packet.ResetBitReader();

            // Two optional strings (CustomerNote and InternalNote based on sub_14248EE80 pattern)
            var customerNoteLength = packet.ReadBits(11);
            packet.ResetBitReader();
            if (customerNoteLength > 0)
                packet.ReadWoWString("CustomerNote", customerNoteLength, indexes);

            var crafterNoteLength = packet.ReadBits(11);
            packet.ResetBitReader();
            if (crafterNoteLength > 0)
                packet.ReadWoWString("CrafterNote", crafterNoteLength, indexes);
        }

        [Parser(Opcode.CMSG_CRAFTING_ORDER_LIST_CRAFTER_ORDERS)]
        public static void HandleCraftingOrderListCrafterOrders(Packet packet)
        {
            packet.ReadPackedGuid128("CrafterGUID");
            packet.ReadInt32("UnknownID");
            packet.ReadByte("OrderType");
            packet.ReadByte("ProfessionID");

            var recipeCount = packet.ReadInt32("RecipeCount");

            var displayID = packet.ReadBits("DisplayID", 2);
            var hasRecraftInfo = packet.ReadBit("HasRecraftInfo");
            packet.ResetBitReader();

            // Each recipe is a SkillLineAbilityID encoded as 20 bits
            for (var i = 0u; i < recipeCount; ++i)
            {
                packet.ReadBits("SkillLineAbilityID", 20, i);
                packet.ResetBitReader();
            }

            // RecraftInfo structure is always present
            ReadCraftingOrderRecraftInfo(packet, "RecraftInfo");
        }

        [Parser(Opcode.SMSG_CRAFTING_ORDER_LIST_ORDERS_RESPONSE)]
        public static void HandleCraftingOrderListOrdersResponse(Packet packet)
        {
            packet.ReadByte("Result");
            var bucketCount = packet.ReadUInt32();
            var orderCount = packet.ReadUInt32();
            packet.ReadUInt32("DesiredDelay");
            packet.ReadUInt32("NumOrders");

            packet.ResetBitReader();
            packet.ReadBit("HasMoreResults");
            packet.ReadBit("IsSorted");

            ReadCraftingOrderClientContext(packet, "ClientContext");

            for (var i = 0u; i < bucketCount; ++i)
                ReadCraftingOrderBucketInfo(packet, "Buckets", i);

            for (var i = 0u; i < orderCount; ++i)
                ReadCraftingOrder(packet, "Orders", i);
        }
    }
}
