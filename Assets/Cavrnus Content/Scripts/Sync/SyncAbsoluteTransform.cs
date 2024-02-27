using CavrnusSdk.API;
using CavrnusSdk.PropertySynchronizers;

namespace CavrnusDemo
{
    public class SyncAbsoluteTransform : CavrnusTransformPropertySynchronizer
    {
        public override CavrnusTransformData GetValue()
        {
            return new CavrnusTransformData(transform.position, transform.eulerAngles, transform.lossyScale);
        }

        public override void SetValue(CavrnusTransformData value)
        {
            transform.position = value.LocalPosition;
            transform.eulerAngles = value.LocalEulerAngles;
            transform.localScale = value.LocalScale;
        }
    }
}