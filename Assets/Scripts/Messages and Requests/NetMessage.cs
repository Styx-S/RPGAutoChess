using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class NetMessage{
    public RequestUser sendTo;
    public MessageBase messageBody;

    public NetMessage(RequestUser sendTo, MessageBase message) {
        this.sendTo = sendTo;
        this.messageBody = message;
    }

    public override string ToString() {
        return "[sendTo:" + "]" + messageBody.ToString();
    }
}
