import { TextField, Button } from '@mui/material';
import StackPage from './stack-page';

function ForgotPassword() {
    return (
        <StackPage title="Reset Password">
            <TextField label="Username" />
            <Button variant="contained" color="primary">Ok</Button>
        </StackPage>
    );
}

export default ForgotPassword