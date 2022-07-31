import { Stack, Typography } from '@mui/material';
import { Error } from '@mui/icons-material';

interface Props {
    text: string;
}

function ErrorMessage(props: Props) {
    const { text } = props;
    return (
        <Stack component="span" spacing={1} direction="row" alignItems="center">
            <Error color="error" />
            <Typography variant="caption" color="error">{text}</Typography>
        </Stack>
    )
}

export default ErrorMessage;