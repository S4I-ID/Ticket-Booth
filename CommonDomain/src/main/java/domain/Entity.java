package domain;

import java.io.Serializable;

public class Entity <E> implements Serializable {
    private E id;

    public Entity(E id) {
        this.id=id;
    }

    public Entity(){}

    public E getID() { return id;}
    public void setID(E id) { this.id=id;}

}