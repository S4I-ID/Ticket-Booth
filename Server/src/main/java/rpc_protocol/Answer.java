package rpc_protocol;

import java.io.Serializable;

public class Answer implements Serializable {
    private AnswerType type;
    private Object data;

    private Answer(){};

    public AnswerType getType() {
        return type;
    }

    public Object getData() {
        return data;
    }

    private void setData(Object data) {
        this.data=data;
    }

    private void setType(AnswerType type) {
        this.type=type;
    }

    @Override
    public String toString() {
        return "Response {"+type+" "+data.toString()+"}";
    }

    public static class Builder {
        private Answer answer = new Answer();

        public Builder type(AnswerType type) {
            answer.setType(type);
            return this;
        }

        public Builder data(Object data) {
            answer.setData(data);
            return this;
        }

        public Answer build() {
            return answer;
        }
    }
}
