import { Button, CircularProgress, TextField, Typography } from '@mui/material';
import StackPage from './stack-page';

function Login() {
    return (
        <StackPage title="Login">
            <TextField label="Username" />
            <TextField label="Password" />
            <CircularProgress />
            <Typography variant="body1" color="red">Username/Password was incorrect</Typography>
            <Button variant="contained" color="primary">Ok</Button>
        </StackPage>
    );
}

export default Login