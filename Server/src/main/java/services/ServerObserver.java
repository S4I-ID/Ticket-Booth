package services;

import domain.Show;

import java.time.LocalDate;
import java.util.List;

public interface ServerObserver {
    void updateShowList(List<Show> shows) throws Exception;
    void updateFilteredList(List<Show> shows) throws Exception;
}
