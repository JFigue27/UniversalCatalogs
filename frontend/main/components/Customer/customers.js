import { Grid, Typography, AppBar, Toolbar } from '@material-ui/core';
import { Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import { Button, InputBase, Icon } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import Dialog from '../../widgets/Dialog';
import Customer from './customer.js';

import CustomersListContainer from './customer.list.container';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {
  limit: 20
  ///start:slot:config<<<///end:slot:config<<<
};

class Customers extends CustomersListContainer {
  constructor(props) {
    super(props, config);
  }

  componentDidMount() {
    console.log('List did mount');
    ///start:slot:didMount<<<
    this.load();
    ///end:slot:didMount<<<
  }

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    return (
      <>
        <Grid className='container-fluid' container direction='column' item xs={12} style={{ padding: 20 }}>
          <Typography variant='h4' className='h4' gutterBottom>
            Customers
          </Typography>
          <Grid container direction='row'>
            <Grid item xs />
            <Pagination
              activePage={this.state.filterOptions.page}
              itemsCountPerPage={this.state.filterOptions.limit}
              totalItemsCount={this.state.filterOptions.totalItems}
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
                        <Grid item>
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
                        fullWidth={true}
                      />
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </Grid>
        <Dialog open={!!this.state.customerDialog} onClose={this.closeDialog} draggable title='Customer'>
          {dialog => {
            return !this.state.isLoading && <Customer dialog={dialog} data={this.state.customerDialog} />;
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
      </>
    );
  }
}

export default Customers;
