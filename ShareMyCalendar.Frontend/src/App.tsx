import { Fragment } from 'react';

import { BrowserRouter, Routes, Route } from 'react-router-dom';

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { CssBaseline } from '@mui/material';

import { ChangePassword, Dashboard, Login, Register, ResetPassword, ConfirmEmail } from './pages';

function App() {
    return (
        <Fragment>
            <CssBaseline/>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Login />} />
                    <Route path="/dashboard" element={<Dashboard />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/confirm-email" element={<ConfirmEmail />} />
                    <Route path="/change-password" element={<ChangePassword />} />
                    <Route path="/reset-password" element={<ResetPassword />} />
                </Routes>
            </BrowserRouter>
        </Fragment>
    );
}

export default App;
