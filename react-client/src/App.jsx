import { useState } from "react";
import axios from "axios";
import "./App.css";

const apiClient = axios.create({
  baseURL: "https://localhost:5000",
  withCredentials: true, // Enable sending cookies with requests
});

function App() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleLogin = async () => {
    try {
      await apiClient.post("/api/login", { username, password });
      setMessage("Logged in successfully");
    } catch (error) {
      setMessage("Login failed");
    }
  };

  const handleLogout = async () => {
    try {
      await apiClient.post("/api/logout");
      setMessage("Logged out successfully");
    } catch (error) {
      setMessage("Logout failed");
    }
  };

  const handleGetProtectedData = async () => {
    try {
      const response = await apiClient.get("/api/protected");
      setMessage(`Protected data: ${response.data}`);
    } catch (error) {
      setMessage("Failed to get protected data");
    }
  };

  return (
    <div className="App">
      <h1>React App</h1>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button onClick={handleLogin}>Login</button>
      <button onClick={handleLogout}>Logout</button>
      <button onClick={handleGetProtectedData}>Get Protected Data</button>
      <p>{message}</p>
    </div>
  );
}

export default App;
