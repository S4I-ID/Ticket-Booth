package service_base.validators;

public interface Validator<T> {
    void validate(T entity) throws Exception;
}
