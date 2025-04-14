import { useState } from "react";
import Alert from "./components/Alert";
import { Button } from "./components/Button";

function App() {
  const [showAlert, setShowAlert] = useState("");

  return (
    <div>
      <h1>Hello World!</h1>
      <Alert onDismiss={() => setShowAlert("")}>{showAlert}</Alert>
      <Button onClick={() => console.log("clicked")}>Click Me</Button>
      <Button style="success" onClick={() => console.log("success!")}>
        Successful
      </Button>
      <Button style="cancel" onClick={() => setShowAlert("You cancelled!")}>
        Cancel
      </Button>
    </div>
  );
}

export default App;
