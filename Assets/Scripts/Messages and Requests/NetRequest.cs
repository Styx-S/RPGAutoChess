using System;

[Serializable]
public class NetRequest {
    public RequestBase requestBody;
    public NetRequest() {

    }

    public NetRequest(RequestBase request) {
        this.requestBody = request;
    }
}
