import { Box, Container, CssBaseline } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";

function App() {
  const [activities, setAcitivites] = useState<Activity[]>([]);

  const [selectedActivity, setSelectedActivity] = useState<
    Activity | undefined
  >(undefined);

  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    axios
      .get<Activity[]>("https://localhost:5001/api/activities")
      .then((response) => setAcitivites(response.data));

    return () => {};
  }, []);

  const handleSelectActivity = (id: string) => {
    setSelectedActivity(activities.find((x) => x.id === id));
  };

  const handleCancelSelectActivity = () => {
    setSelectedActivity(undefined);
  };

  const handleOpenForm = (id?: string) => {
    if (id) handleSelectActivity(id);
    else handleCancelSelectActivity();

    setEditMode(true);
  };

  const handleFormClose = () => {
    setEditMode(false);
  };

  const handleSubmitForm = (activity: Activity) => {
    if (activity.id) {
      setAcitivites(
        activities.map((x) => (x.id === activity.id ? activity : x))
      );
    } else {
      const newActivity = { ...activity, id: activities.length.toString() };
      setSelectedActivity(newActivity);
      setAcitivites([...activities, newActivity]);
    }
    setEditMode(false);
  };

  const handleDelete = (id: string) => {
    setAcitivites(activities.filter((x) => x.id !== id));
  };

  return (
    <Box sx={{ bgcolor: "#eee" }}>
      <CssBaseline />

      <NavBar openForm={handleOpenForm} />

      <Container maxWidth="xl" sx={{ mt: 3 }}>
        <ActivityDashboard
          activities={activities}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          selectedActivity={selectedActivity}
          editMode={editMode}
          openForm={handleOpenForm}
          closeForm={handleFormClose}
          submitForm={handleSubmitForm}
          deleteActivity={handleDelete}
        ></ActivityDashboard>
      </Container>
    </Box>
  );
}

export default App;
