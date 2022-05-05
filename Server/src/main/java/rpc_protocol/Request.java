package rpc_protocol;

import java.io.Serializable;

public class Request implements Serializable {
    private RequestType type;
    private Object data;

    private Request(){};

    public RequestType getType() {
        return type;
    }

    public Object getData() {
        return data;
    }

    private void setData(Object data) {
        this.data=data;
    }

    private void setType(RequestType type) {
        this.type=type;
    }

    @Override
    public String toString() {
        return "Response {"+type+" "+data.toString()+"}";
    }

    public static class Builder {
        private Request request = new Request();

        public Builder type(RequestType type) {
            request.setType(type);
            return this;
        }

        public Builder data(Object data) {
            request.setData(data);
            return this;
        }

        public Request build() {
            return request;
        }
    }
}
