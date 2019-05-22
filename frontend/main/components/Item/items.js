import { Grid, Typography, AppBar, Toolbar } from '@material-ui/core';
import { Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import { Button, Icon } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import moment from 'moment';
import ItemsListContainer from './item.list.container';
import Dialog from '../../widgets/Dialog';
import Item from './item.form';
///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {
  limit: 20
};

class Items extends ItemsListContainer {
  constructor(props) {
    super(props, config);
  }

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    return (
      <>
        <Grid className='container-fluid' container direction='column' item xs={12} style={{ padding: 20 }}>
          <Typography variant='h2' className='' gutterBottom>
            OEEs
          </Typography>
          <pre>{JSON.stringify(this.state, null, 3)}</pre>
          <Grid container direction='row'>
            <Grid item xs />
            <Pagination
              activePage={this.state.filterOptions.page}
              itemsCountPerPage={this.state.filterOptions.limit}
              totalItemsCount={this.state.filterOptions.totalItems}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.state.pageChanged(newPage);
              }}
            />
          </Grid>
          <Table className=''>
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
                      <Grid container direction='row' className='row' justify='center' alignItems='center' spacing={8}>
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
                    <TableCell>{item.ItemNumber}</TableCell>
                    <TableCell>{item.ItemDescription}</TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </Grid>
        <Dialog open={this.state.itemIsOpen} onClose={this.closeItemDialog} maxWidth={'sm'} ok={this.onOkItemDialog} draggable>
          <Item />
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

export default Items;
