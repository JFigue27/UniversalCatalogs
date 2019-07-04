import { NoSsr, Grid, Typography } from '@material-ui/core';
import { Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import { Button } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import { Icon } from '@material-ui/core';
import { InputBase } from '@material-ui/core';
import Dialog from '../../widgets/Dialog';
import Employee from './employee.js';
import { AppBar, Toolbar } from '@material-ui/core';

import EmployeesListContainer from './employee.list.container';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {
  limit: 20
  ///start:slot:config<<<///end:slot:config<<<
};

class Employees extends EmployeesListContainer {
  constructor(props) {
    super(props, config);
  }

  componentDidMount() {
    console.log('List did mount');
    this.load();
    ///start:slot:didMount<<<
    this.load();
    ///end:slot:didMount<<<
  }

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    return (
      <NoSsr>
        <Grid className='container-fluid' container direction='column' item xs={12} style={{ padding: 20 }}>
          <Typography variant='h4' className='h4' gutterBottom>
            Employees
          </Typography>
          <Grid container direction='row'>
            <Grid item xs />
            <Pagination
              activePage={this.filterOptions.page}
              itemsCountPerPage={this.filterOptions.limit}
              totalItemsCount={this.filterOptions.totalItems}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage);
              }}
            />
          </Grid>
          <Table className='' size='small'>
            <TableHead>
              <TableRow>
                <TableCell />
                <TableCell>Value</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {this.state.baseList &&
                this.state.baseList.map(item => (
                  <TableRow key={item.Id}>
                    <TableCell>
                      <Grid container direction='row' className='row' justify='center' alignItems='center' spacing={2}>
                        <Grid item xs>
                          <Button
                            variant='contained'
                            color='default'
                            className=''
                            onClick={event => {
                              this.openItem(event, item);
                            }}
                            size='small'
                          >
                            <Icon>edit</Icon>Open
                          </Button>
                        </Grid>
                      </Grid>
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'Value')}
                        value={item.Value || ''}
                        fullWidth
                      />
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </Grid>
        <Dialog open={!!this.state.employee} onClose={this.closeDialog} draggable title='Employee' okLabel='Save'>
          {dialog => {
            return !this.state.isLoading && <Employee dialog={dialog} data={this.state.employee} />;
          }}
        </Dialog>
        <AppBar position='fixed' style={{ top: 'auto', bottom: 0 }}>
          <Toolbar variant='dense'>
            <SearchBox bindFilterInput={this.bindFilterInput} />
            <Grid item xs />
            <Button
              variant='contained'
              color='default'
              className=''
              onClick={event => {
                this.createInstance(event, {});
              }}
            >
              <Icon>add_circle</Icon>New
            </Button>
          </Toolbar>
        </AppBar>
      </NoSsr>
    );
  }
}

export default Employees;
