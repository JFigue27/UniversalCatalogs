import { NoSsr, Grid, Typography } from '@material-ui/core';
import { Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import { Button } from '@material-ui/core';
import SearchBox from '../../widgets/searchbox';
import Pagination from 'react-js-pagination';
import { Icon } from '@material-ui/core';
import { InputBase } from '@material-ui/core';
import Dialog from '../../widgets/Dialog';
import ItemForm from './item.form.js';
import { AppBar, Toolbar } from '@material-ui/core';

import ItemsListContainer from './item.list.container';
///start:slot:dependencies<<<
import { TableFooter, TablePagination } from '@material-ui/core';
// import { PagerComponent } from '@syncfusion/ej2-react-grids';
///end:slot:dependencies<<<

const config = {
  limit: 20
  ///start:slot:config<<<///end:slot:config<<<
};

class Items extends ItemsListContainer {
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
      <NoSsr>
        <Grid className='container-fluid' container direction='column' item xs={12} style={{ padding: 20 }}>
          <Typography variant='h4' className='' gutterBottom>
            Items
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
                <TableCell>Item Number</TableCell>
                <TableCell>Item Description</TableCell>
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
                        onChange={event => this.handleInputChange(event, 'ItemNumber')}
                        value={item.ItemNumber || ''}
                        fullWidth
                      />
                    </TableCell>
                    <TableCell>
                      <InputBase
                        type='text'
                        className=''
                        autoComplete='off'
                        disabled={this.isDisabled}
                        readOnly={true}
                        onChange={event => this.handleInputChange(event, 'ItemDescription')}
                        value={item.ItemDescription || ''}
                        fullWidth
                      />
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </Grid>
        <Dialog open={!!this.state.item} onClose={this.closeDialog} draggable title='Item' okLabel='Save'>
          {dialog => {
            return !this.state.isLoading && <ItemForm dialog={dialog} data={this.state.item} />;
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

export default Items;
