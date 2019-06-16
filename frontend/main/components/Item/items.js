import { Grid, Typography, AppBar, Toolbar, TableFooter } from '@material-ui/core';
import { Table, TableHead, TableRow, TableCell, TableBody, TablePagination } from '@material-ui/core';
import { Button, Icon } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import ItemsListContainer from './item.list.container';
import Dialog from '../../widgets/Dialog';
import ItemForm from './item.form.js';
// import { PagerComponent } from '@syncfusion/ej2-react-grids';
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
            Items
          </Typography>
          <Grid container direction='row'>
            <Grid item xs />
            {/* <PagerComponent
              currentPage={this.state.filterOptions.page}
              pageSize={this.state.filterOptions.limit}
              totalRecordsCount={this.state.filterOptions.totalItems}
              pageCount={5}
              pageSizes={true}
              enableExternalMessage={false}
              click={pageEvent => {
                this.pageChanged(pageEvent.currentPage);
              }}
            /> */}
            {/* <Pagination
              activePage={this.state.filterOptions.page}
              itemsCountPerPage={this.state.filterOptions.limit}
              totalItemsCount={this.state.filterOptions.totalItems}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage);
              }}
            /> */}
          </Grid>
          <Table className='' size='small'>
            <TableHead>
              {/* <TableRow>
                <TablePagination
                  count={this.state.filterOptions.totalItems || 0}
                  page={this.state.filterOptions.page - 1}
                  rowsPerPage={this.state.filterOptions.limit}
                  onChangeRowsPerPage={event => {
                    this.pageChanged(this.state.filterOptions.page + 1, event.target.value);
                  }}
                  onChangePage={(event, page) => {
                    console.log(event, page);
                    this.pageChanged(page + 1);
                  }}
                />
              </TableRow> */}
              <TableRow>
                <TableCell style={{ width: 150 }} />
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
                        {item.itemIndex}
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
            <TableFooter>
              <TableRow>
                <TablePagination
                  count={this.state.filterOptions.totalItems || 0}
                  page={this.state.filterOptions.page - 1}
                  rowsPerPage={this.state.filterOptions.limit}
                  onChangeRowsPerPage={event => {
                    this.pageChanged(1, event.target.value);
                  }}
                  onChangePage={(event, page) => {
                    console.log(event, page);
                    this.pageChanged(page + 1);
                  }}
                />
              </TableRow>
            </TableFooter>
          </Table>
          <Grid container direction='row'>
            <Grid item xs />

            {/* <PagerComponent
              currentPage={this.state.filterOptions.page}
              pageSize={this.state.filterOptions.limit}
              totalRecordsCount={this.state.filterOptions.totalItems}
              pageCount={5}
              pageSizes={true}
              enableExternalMessage={false}
              click={pageEvent => {
                this.pageChanged(pageEvent.currentPage);
              }}
            /> */}

            {/* <Pagination
              activePage={this.state.filterOptions.page}
              itemsCountPerPage={this.state.filterOptions.limit}
              totalItemsCount={this.state.filterOptions.totalItems}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage);
              }}
            /> */}
          </Grid>
        </Grid>

        <Dialog open={!!this.state.itemDialog} onClose={this.closeDialog} draggable title='Item'>
          {dialog => <ItemForm dialog={dialog} data={this.state.itemDialog} />}
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
