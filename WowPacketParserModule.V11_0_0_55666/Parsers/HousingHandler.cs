using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V11_0_0_55666.Parsers
{
    public static class HousingHandler
    {
        [Parser(Opcode.CMSG_HOUSING_DECOR_REQUEST_STORAGE)]
        public static void HousingDecorRequestStorage(Packet packet)
        {
            packet.ReadPackedGuid128("BnetAccountGUID");
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_DELETE_FROM_STORAGE)]
        public static void HandleHousingDecorDeleteFromStorage(Packet packet)
        {
            var count = packet.ReadBits(5);
            for (var i = 0; i < count; ++i)
                packet.ReadPackedGuid128("DecorGUID", i);
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_DELETE_FROM_STORAGE_BY_ID)]
        public static void HandleHousingDecorDeleteFromStorageById(Packet packet)
        {
            packet.ReadUInt32("DecorID");
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_SET_DYE_SLOTS)]
        public static void HousingDecorSetDyeSlots(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            for (var i = 0; i < 3; ++i)
            {
                packet.ReadInt32("DyeColorID", i);
            }
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_MOVE)]
        public static void HandleHousingDecorMove(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadVector3("Position");
            packet.ReadVector3("Rotation");
            packet.ReadSingle("Scale");
            packet.ReadPackedGuid128("ParentDecorGUID");
            packet.ReadPackedGuid128("RoomGUID");
            packet.ReadPackedGuid128("ParentHouseFixtureGUID");
            packet.ReadInt32("PlacedComponentID");
            packet.ReadByte("AddedFlags");
            packet.ReadByte("RemovedFlags");
            packet.ReadBit("IncludeChildren");
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_REMOVE)]
        public static void HousingDecorRemove(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_LOCK)]
        public static void HousingDecorLock(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadBit("LockRequest");
            packet.ReadBit("IncludeChildren");
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_PLACE)]
        public static void HousingDecorPlace(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadVector3("Position");
            packet.ReadVector3("Rotation");
            packet.ReadSingle("Scale");
            packet.ReadPackedGuid128("AttachParentGUID");
            packet.ReadPackedGuid128("RoomGUID");
            packet.ReadPackedGuid128("ParentHouseFixtureGUID");
            packet.ReadInt32("PlacedComponentID");
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_REDEEM_DEFERRED_DECOR)]
        public static void HousingDecorRedeemDeferredDecor(Packet packet)
        {
            packet.ReadUInt32("DecorID");
            packet.ReadUInt32("TransactionID");
        }

        [Parser(Opcode.CMSG_HOUSING_GET_PLAYER_PERMISSIONS)]
        public static void HousingGetPlayerPermission(Packet packet)
        {
            var hasHouseGUID = packet.ReadBit("HasHouseGUID");
            if (hasHouseGUID)
                packet.ReadPackedGuid128("HouseGUID");
        }

        [Parser(Opcode.CMSG_HOUSING_ROOM_REMOVE)]
        public static void HandleHousingRoomRemove(Packet packet)
        {
            packet.ReadPackedGuid128("RoomGUID");
        }

        [Parser(Opcode.CMSG_HOUSING_ROOM_ROTATE)]
        public static void HousingRoomRotate(Packet packet)
        {
            packet.ReadPackedGuid128("RoomGUID");
            packet.ReadBit("RotateRight");
        }

        [Parser(Opcode.CMSG_HOUSING_SVCS_PLAYER_VIEW_HOUSES_BY_PLAYER)]
        public static void HandleHousingSvcsPlayerViewHousesByPlayer(Packet packet)
        {
            packet.ReadPackedGuid128("TargetPlayerGUID");
        }

        [Parser(Opcode.CMSG_HOUSING_SVCS_GET_POTENTIAL_HOUSE_OWNERS)]
        public static void HandleHousingSvcsGetPotentialHouseOwners(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
        }

        [Parser(Opcode.CMSG_HOUSING_SVCS_GET_BNET_FRIEND_NEIGHBORHOODS)]
        public static void HandleHousingSvcsGetBneFriendNeighborhoods(Packet packet)
        {
            packet.ReadPackedGuid128("FriendBNetAccountGUID");
        }

        [Parser(Opcode.CMSG_NEIGHBORHOOD_OPEN_CORNERSTONE_UI)]
        public static void HandleNeighborhoodOpenCornerstoneUi(Packet packet)
        {
            packet.ReadUInt32("PlotID");
            packet.ReadPackedGuid128("CornerstoneGUID");
        }

        [Parser(Opcode.CMSG_QUERY_NEIGHBORHOOD_INFO)]
        public static void HandleQueryNeighborhoodInfo(Packet packet)
        {
            packet.ReadPackedGuid128("NeighborhoodGUID");
        }

        [Parser(Opcode.SMSG_NEIGHBORHOOD_PLAYER_ENTER_PLOT)]
        public static void HandleNeighborhoodPlayerEnterPlot(Packet packet)
        {
            packet.ReadPackedGuid128("AreaTriggerGUID");
        }

        [Parser(Opcode.SMSG_HOUSING_GET_CURRENT_HOUSE_INFO_RESPONSE)]
        public static void HandleHousingGetCurrentHouseInfoResponse(Packet packet)
        {
            ReadHouse(packet, "House");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSING_GET_PLAYER_PERMISSIONS_RESPONSE)]
        public static void HousingGetPlayerPermissionResponse(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadByte("Field_09");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_REQUEST_STORAGE_RESPONSE)]
        public static void HousingDecorRequestStorageResponse(Packet packet)
        {
            packet.ReadPackedGuid128("BnetAccountGUID");
            packet.ReadByteE<HousingResult>("Result");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V12_0_0_65390))
                packet.ReadBit("NewDataPulled");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_SYSTEM_SET_DYE_SLOTS_RESPONSE)]
        public static void HandleHousingDecorSystemSetDyeSlotsResponse(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_MOVE_RESPONSE)]
        public static void HandleHousingDecorMoveResponse(Packet packet)
        {
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadUInt32("ClientDelay");
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadBit("IncludeChildren");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_REMOVE_RESPONSE)]
        public static void HandleHousingDecorRemoveResponse(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadUInt32("ClientDelay");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_LOCK_RESPONSE)]
        public static void HandleHousingDecorLockResponse(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadUInt32("ClientDelay");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadBit("IsLocked");
            packet.ReadBit("IncludeChildren");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_SET_EDIT_MODE_RESPONSE)]
        public static void HandleHousingDecorSetEditModeResponse(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
            packet.ReadPackedGuid128("HouseOwnerAccountGUID");
            var playersEditing = packet.ReadUInt32("PlayerGUIDEditingCount");
            packet.ReadByteE<HousingResult>("Result");

            for (var i = 0; i < playersEditing; ++i)
                packet.ReadPackedGuid128("PlayerGUIDEditing", i);
        }

        [Parser(Opcode.SMSG_HOUSING_REDEEM_DEFERRED_DECOR_RESPONSE)]
        public static void HandleHousingRedeemDeferredDecorResponse(Packet packet)
        {
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadUInt32("TransactionID");
        }

        [Parser(Opcode.SMSG_HOUSING_DECOR_PLACE_RESPONSE)]
        public static void HandleHousingDecorPlaceResponse(Packet packet)
        {
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadUInt32("ClientDelay");
            packet.ReadPackedGuid128("DecorGUID");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSE_EXTERIOR_LOCK_RESPONSE)]
        public static void HandleHousingExteriorLockResponse(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadBit("IsLocked");
        }

        [Parser(Opcode.SMSG_HOUSING_FIXTURE_SET_EDIT_MODE_RESPONSE)]
        public static void HandleHousingFixtureSetEditModeResponse(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
            packet.ReadPackedGuid128("PlayerGUIDEditing");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSING_ROOM_REMOVE_RESPONSE)]
        public static void HandleHousingRoomRemoveResponse(Packet packet)
        {
            packet.ReadPackedGuid128("RoomGUID");
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSING_ROOM_SET_LAYOUT_EDIT_MODE_RESPONSE)]
        public static void HandleHousingRoomSetLayoutEditModeResponse(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadBit("Enabled");
        }

        [Parser(Opcode.SMSG_HOUSING_ROOM_UPDATE_RESPONSE)]
        public static void HousingRoomUpdateResponse(Packet packet)
        {
            packet.ReadPackedGuid128("SourceRoomGUID");
            packet.ReadByteE<HousingResult>("Result");
        }

        [Parser(Opcode.SMSG_HOUSING_HOUSE_STATUS_RESPONSE)]
        public static void HandleHousingHouseStatusResponse(Packet packet)
        {
            packet.ReadPackedGuid128("HouseGUID");
            packet.ReadPackedGuid128("HouseOwnerAccountGUID");
            packet.ReadPackedGuid128("HouseOwnerGUID");
            packet.ReadPackedGuid128("LockedDecorGUID");
            packet.ReadByteE<HousingResult>("Result");
            packet.ReadBit("DecorEditModeEnabled");
            packet.ReadBit("LayoutEditModeEnabled");
            packet.ReadBit("FixtureEditModeEnabled");
        }

        [Parser(Opcode.SMSG_HOUSING_SVCS_PLAYER_VIEW_HOUSES_RESPONSE)]
        public static void HandleHousingSvcsPlayerViewHousesResponse(Packet packet)
        {
            var count =  packet.ReadUInt32("TargetHousesCount");
            packet.ReadByteE<HousingResult>("Result");
            for (uint i = 0; i < count; i++)
                ReadHouse(packet, "TargetHouse", i);
        }

        [Parser(Opcode.SMSG_HOUSING_SVCS_GET_PLAYER_HOUSES_INFO_RESPONSE)]
        public static void HandleHousingSvcsGetPlayerHousesInfoResponse(Packet packet)
        {
            var count =  packet.ReadUInt32("PlayerHousesCount");
            packet.ReadByteE<HousingResult>("Result");
            for (uint i = 0; i < count; i++)
                ReadHouse(packet, "PlayerHouse", i);
        }

        [Parser(Opcode.SMSG_INVALIDATE_NEIGHBORHOOD_NAME)]
        public static void HandleInvalidateNeighborhoodName(Packet packet)
        {
            packet.ReadPackedGuid128("NeighborhoodGUID");
        }

        [Parser(Opcode.SMSG_QUERY_NEIGHBORHOOD_NAME_RESPONSE)]
        public static void HandleQueryNeighborhoodNameResponse(Packet packet)
        {
            packet.ReadPackedGuid128("NeighborhoodGUID");
            var allow = packet.ReadBit("Allow");
            if (!allow)
                return;

            packet.ResetBitReader();
            var nameLen = packet.ReadBits(8);
            packet.ReadWoWString("NeighborhoodName", nameLen);
        }

        [Parser(Opcode.CMSG_HOUSING_DECOR_SET_EDIT_MODE)]
        [Parser(Opcode.CMSG_HOUSING_FIXTURE_SET_EDIT_MODE)]
        [Parser(Opcode.CMSG_HOUSING_ROOM_SET_LAYOUT_EDIT_MODE)]
        public static void HandleHousingSetEditMode(Packet packet)
        {
            packet.ReadBit("IsEnabled");
        }

        [Parser(Opcode.CMSG_HOUSING_HOUSE_STATUS)]
        [Parser(Opcode.CMSG_HOUSE_INTERIOR_LEAVE_HOUSE)]
        [Parser(Opcode.CMSG_HOUSING_GET_CURRENT_HOUSE_INFO)]
        [Parser(Opcode.CMSG_HOUSING_SVCS_GET_PLAYER_HOUSES_INFO)]
        [Parser(Opcode.CMSG_HOUSING_SVCS_GET_HOUSE_FINDER_INFO)]
        [Parser(Opcode.SMSG_NEIGHBORHOOD_PLAYER_LEAVE_PLOT)]
        public static void HandleHousingNull(Packet packet)
        {
        }

        private static void ReadHouse(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();
            packet.ReadPackedGuid128("GUID", indexes);
            packet.ReadPackedGuid128("CosmeticOwner", indexes);
            packet.ReadPackedGuid128("NeighborhoodGUID", indexes);

            packet.ReadByte("PlotID", indexes);
            packet.ReadUInt32("HouseSettingFlags", indexes);

            var hasHasReservationTime = packet.ReadBit("HasReservationTime", indexes);
            if (hasHasReservationTime)
                packet.ReadTime64("HasReservationTime", indexes);
        }
    }
}
