using System.Net;
using System.Net.Sockets;

namespace Adbliterator.utilities.blocking;

public class ResponseHandler {
    public static void SendBlockedResponse(UdpClient udpClient, IPEndPoint clientEndpoint, byte[] requestData) {
        var response = new byte[requestData.Length];
        Buffer.BlockCopy(requestData, 0, response, 0, requestData.Length);

        response[2] = 1 << 7;
        response[3] = 3;

        udpClient.Send(response, response.Length, clientEndpoint);
    }
}