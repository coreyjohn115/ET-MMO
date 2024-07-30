using UnityEngine.Networking;

namespace ET.Client
{
    public class AcceptAllCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}