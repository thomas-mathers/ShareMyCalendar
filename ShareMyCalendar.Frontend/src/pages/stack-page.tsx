import { PropsWithChildren } from 'react';
import { Container, Stack, Typography } from '@mui/material';

interface Props {
    title: string;
}

function StackPage(props: PropsWithChildren<Props>) {
    const { title, children } = props;
    return (
        <Container maxWidth="xs">
            <Stack spacing={4}>
                <Typography variant="h4" component="h1">{title}</Typography>
                {children}
            </Stack>
        </Container>
    );
}

export default StackPage;