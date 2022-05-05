package service_base;

import domain.Show;
import repository.ShowDatabaseRepository;
import service_base.validators.ShowValidator;

import java.time.LocalDate;
import java.util.List;
import java.util.stream.Collectors;

public class ShowService {
    private ShowDatabaseRepository repository;
    private final ShowValidator validator;

    public ShowService(ShowDatabaseRepository repository) {
        this.repository = repository;
        this.validator = new ShowValidator();
    }

    public List<Show> getAllShows() throws Exception {
        return repository.getAll();
    }

    public List<Show> getShowsByDate(LocalDate date) throws Exception {

        List<Show> showsOnDate = repository.getAll()
                .stream()
                .filter(show -> show.getStartTime().getYear()==date.getYear()&&
                                show.getStartTime().getMonthValue()==date.getMonthValue()&&
                                show.getStartTime().getDayOfMonth()==date.getDayOfMonth())
                .collect(Collectors.toList());

        return showsOnDate;
    }

    public void addShow (Show show) throws Exception {
        validator.validate(show);
        repository.add(show);
    }

    public Show findShow(int showId) throws Exception {
        return repository.find(showId);
    }

    public Show updateShow(Show newShow) throws Exception {
        validator.validate(newShow);
        return repository.update(newShow);
    }
}
