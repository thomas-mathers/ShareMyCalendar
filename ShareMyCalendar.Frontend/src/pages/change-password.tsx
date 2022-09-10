import { Button, TextField } from '@mui/material';
import StackPage from './stack-page';

function ChangePassword() {
  return (
    <StackPage title="Change Password">
      <TextField label="Current Password" />
      <TextField label="New Password" />
      <TextField label="Confirm New Password" />
      <Button variant="contained" color="primary">Ok</Button>
    </StackPage>
  );
}

export default ChangePassword