import App from '../components/App';
import { Grid, Button, Paper, Link } from '@material-ui/core';

export default () => (
  <App>
    <Grid container direction='column' justify='center' alignItems='center'>
      <Paper style={{ padding: '4%' }}>
        <Grid item xs={12}>
          <img src='/static/images/Molex_Black.png' alt='Molex Logo' style={{ width: 350 }} />
        </Grid>
        <Grid item xs={12} sm container style={{ padding: 25 }}>
          <Grid item xs container direction='row' justify='center' alignItems='center'>
            <Link href='/catalogs' className='app-link-button'>
              <Button variant='contained'>Catalogs</Button>
            </Link>
          </Grid>
        </Grid>
      </Paper>
    </Grid>
  </App>
);
