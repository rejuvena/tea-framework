using System.IO;

namespace TeaFramework.API.Features.Packets
{
    /// <summary>
    ///     Represents serializable packet data.
    /// </summary>
    public interface IPacketData
    {
        void SerializePacket(BinaryWriter writer);
    }
}