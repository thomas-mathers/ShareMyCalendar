import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ChangePassword, Home, Login, ResetPassword, ConfirmEmail } from './pages';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/confirm-email" element={<ConfirmEmail />} />
                <Route path="/change-password" element={<ChangePassword />} />
                <Route path="/reset-password" element={<ResetPassword />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;
