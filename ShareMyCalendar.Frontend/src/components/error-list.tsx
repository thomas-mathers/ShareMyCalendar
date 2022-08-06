import { Box } from '@mui/material';
import ErrorMessage from './error-message';

interface Props {
  errors: string[];
}

function ErrorList(props: Props) {
  const { errors } = props;
  return (
    <Box>
      {errors.map((e, i) => <ErrorMessage key={i} text={e} />)}
    </Box>
  )
}

export default ErrorList;