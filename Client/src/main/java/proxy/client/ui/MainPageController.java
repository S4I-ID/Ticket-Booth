package proxy.client.ui;

import javafx.application.Platform;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import domain.Show;
import services.MainServerServiceInterface;
import services.ServerObserver;

import java.time.LocalDate;
import java.util.List;

public class MainPageController implements ServerObserver
{
    @FXML
    private Button buyButton;
    @FXML
    private Button filterButton;
    @FXML
    private Button logoutButton;
    @FXML
    private TableView<Show> showsTableView;
    @FXML
    private TableView<Show> filteredShowsTableView;
    @FXML
    private TextField nameTextField;
    @FXML
    private TextField ticketTextField;
    @FXML
    private DatePicker datePicker;
    @FXML
    private TableColumn<Show,String> artistName;
    @FXML
    private TableColumn<Show,String> showName;
    @FXML
    private TableColumn<Show,String> address;
    @FXML
    private TableColumn<Show,String> startTime;
    @FXML
    private TableColumn<Show,Integer> availableSeats;
    @FXML
    private TableColumn<Show,Integer> soldSeats;
    @FXML
    private TableColumn<Show,String> artistName1;
    @FXML
    private TableColumn<Show,String> showName1;
    @FXML
    private TableColumn<Show,String> address1;
    @FXML
    private TableColumn<Show,String> startTime1;
    @FXML
    private TableColumn<Show,Integer> availableSeats1;
    @FXML
    private TableColumn<Show,Integer> soldSeats1;


    private ObservableList<Show> allShowsList = FXCollections.observableArrayList();
    private ObservableList<Show> filteredShowsList = FXCollections.observableArrayList();
    private LocalDate date;

    private MainServerServiceInterface service;
    String username;

    public void setUsername(String username) {
        this.username=username;
    }

    public MainPageController(MainServerServiceInterface service) {
        this.service = service;
        this.date=null;
    }

    @FXML
    public void initialize() {
        Platform.runLater(this::initializeTables);
    }

    private void initializeTables() {
        artistName.setCellValueFactory(new PropertyValueFactory<Show,String>("artistName"));
        showName.setCellValueFactory(new PropertyValueFactory<Show,String>("showName"));
        address.setCellValueFactory(new PropertyValueFactory<Show,String>("address"));
        startTime.setCellValueFactory(new PropertyValueFactory<Show,String>("startTime"));
        availableSeats.setCellValueFactory(new PropertyValueFactory<Show,Integer>("availableSeats"));
        soldSeats.setCellValueFactory(new PropertyValueFactory<Show,Integer>("soldSeats"));
        artistName1.setCellValueFactory(new PropertyValueFactory<Show,String>("artistName"));
        showName1.setCellValueFactory(new PropertyValueFactory<Show,String>("showName"));
        address1.setCellValueFactory(new PropertyValueFactory<Show,String>("address"));
        startTime1.setCellValueFactory(new PropertyValueFactory<Show,String>("startTime"));
        availableSeats1.setCellValueFactory(new PropertyValueFactory<Show,Integer>("availableSeats"));
        soldSeats1.setCellValueFactory(new PropertyValueFactory<Show,Integer>("soldSeats"));
        try {   // POPULATE MAIN TABLE AT STARTUP
            updateShowList(service.getAllShows());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void updateMainTable(List<Show> shows) {
        try {
            allShowsList.setAll(shows);
            showName.setCellFactory(column -> { // SET ROW BACKGROUND TO RED IF NO TICKETS LEFT
                return new TableCell<Show, String>() {
                    @Override
                    protected void updateItem(String item, boolean empty) {
                        super.updateItem(item, empty);
                        setText(empty ? "" : getItem().toString());
                        setStyle(null);
                        setGraphic(null);

                        TableRow<Show> currentRow = getTableRow();

                        if (!empty && currentRow.getItem()!=null)
                        {
                            if (currentRow.getItem().getSoldSeats().equals(currentRow.getItem().getAvailableSeats())) {
                                currentRow.setStyle("-fx-background-color:red");
                            }
                            else
                                currentRow.setStyle(null);
                        }}
                };
            });
        }
        catch (Exception e) {
            WarningBoxUI.show(e.getMessage());
        }
        showsTableView.setItems(allShowsList);
        System.out.println("Updated main table");
        if (date!=null) {   // IF RECEIVED UPDATE FOR MAIN TABLE, ASK FOR FILTERED TABLE UPDATES
            try {
                updateFilteredList(service.getShowsByDate(date));
            }
            catch (Exception ignored) {}
        }
    }

    @Override
    public void updateFilteredList(List<Show> shows) {  // RUN.LATER HANDLER FOR UPDATE
        Platform.runLater(()->updateFilteredTable(shows));
    }

    @Override
    public void updateShowList(List<Show> shows) {  // RUN.LATER HANDLER FOR UPDATE
        Platform.runLater(()->updateMainTable(shows));
    }

    private void updateFilteredTable(List<Show> shows) {
        try {
            System.out.println("Updating filtered table...");
            filteredShowsList.setAll(shows);
            showName1.setCellFactory(column -> {
                return new TableCell<Show, String>() {
                    @Override
                    protected void updateItem(String item, boolean empty) {
                        super.updateItem(item, empty);

                        setText(empty ? "" : getItem().toString());
                        setStyle(null);
                        setGraphic(null);

                        TableRow<Show> currentRow = getTableRow();

                        if (!empty && currentRow.getItem()!=null)
                        {
                            if (currentRow.getItem().getSoldSeats().equals(currentRow.getItem().getAvailableSeats())) {
                                currentRow.setStyle("-fx-background-color:red");
                            }
                            else
                                currentRow.setStyle(null);
                        }}
            };});
        } catch (Exception e) {
            WarningBoxUI.show(e.getMessage());
        }
        filteredShowsTableView.setItems(filteredShowsList);
        System.out.println("Updated filtered table");
    }

    @FXML
    protected void buyButtonAction(ActionEvent event) {
        String buyerName = nameTextField.getText();
        try {
            int tickets = Integer.parseInt(ticketTextField.getText());
            Show show = showsTableView.getSelectionModel().getSelectedItem();
            if (show==null)
                show = filteredShowsTableView.getSelectionModel().getSelectedItem();
            if (show==null)
                WarningBoxUI.show("No show selected!\n");
            else {
                if (buyerName.equals(""))
                    WarningBoxUI.show("Name or ticket number is null!\n");
                else {
                    service.addSaleToShow(buyerName, tickets, show.getID());
                    // DO NOT ADD UPDATES HERE, LET SERVER HANDLE THEM
                }
            }
        }
        catch (Exception e) {
            WarningBoxUI.show("Invalid sale!\n"+e.getMessage());
        }
    }

    @FXML
    protected void filterButtonAction(ActionEvent event) {
        date = datePicker.getValue();
        if (date==null) {   // IF NOTHING SELECTED
            WarningBoxUI.show("Invalid date!\n");
        }
        else {
            try {
                updateFilteredTable(service.getShowsByDate(date));
            } catch (Exception e) {
                WarningBoxUI.show(e.getMessage());
            }
        }
    }

    @FXML
    protected void logoutButtonAction (ActionEvent event) {
        try {
            service.logout(username,this);
            System.exit(0);
        } catch (Exception e) {
            WarningBoxUI.show(e.getMessage());
        }
    }
}
