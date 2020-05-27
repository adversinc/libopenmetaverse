using System;
using OpenMetaverse;

namespace OpenMetaverse.Assets
{
    /// <summary>
    /// Represents an asset which type is not important for us
    /// </summary>
    public class AssetGeneric: Asset
    {
        /// <summary>Override the base classes AssetType</summary>
        public override AssetType AssetType { get { return AssetType.Unknown; } }

        /// <summary>Default Constructor</summary>
        public AssetGeneric() { }

        /// <summary>
        /// Construct an Asset object
        /// </summary>
        /// <param name="assetID">A unique <see cref="UUID"/> specific to this asset</param>
        /// <param name="assetData">A byte array containing the raw asset data</param>
        public AssetGeneric(UUID assetID, byte[] assetData)
            : base(assetID, assetData)
        {
        }

        public override void Encode() { }
        public override bool Decode() { return true; }
    }
}
