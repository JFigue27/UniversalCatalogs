import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid, Paper } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import ListContainer from '../../core/ListContainer';
import { Container } from '@material-ui/core';
import { Table } from '@material-ui/core';
import { TableHead } from '@material-ui/core';
import { TableBody } from '@material-ui/core';
import { TableRow } from '@material-ui/core';
import { TableCell } from '@material-ui/core';
import { Button } from '@material-ui/core';
import { Icon } from '@material-ui/core';
import { InputBase } from '@material-ui/core';

import Dialog from '../../widgets/Dialog';
import Catalog from './catalog.js';
import { AppBar, Toolbar } from '@material-ui/core';

import CatalogService from './catalog.service';
///start:slot:dependencies<<<
import CatalogTypeService from '../CatalogType/catalogtype.service';
const catalogTypeService = new CatalogTypeService();
///end:slot:dependencies<<<

const service = new CatalogService();
const defaultConfig = {
  service,
  ///start:slot:config<<<
  filterName: 'Catalog'
  ///end:slot:config<<<
};

class CatalogsList extends ListContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
  }

  componentDidMount() {
    console.log('List did mount');
    this.load(this.props.router.query);
    ///start:slot:didMount<<<
    catalogTypeService.GetSingleWhere('Name', this.props.router.query.name).then(response => {
      this.setState({ additionalFields: response.ConvertedFields, parentType: response.ParentType });
    });
    ///end:slot:didMount<<<
  }

  AFTER_LOAD = () => {
    console.log('AFTER_LOAD');
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<
    instance.CatalogType = this.props.router.query.name;
    this.openDialog(instance);
    ///end:slot:afterCreate<<<
  };

  AFTER_CREATE_AND_CHECKOUT = entity => {
    console.log('AFTER_CREATE_AND_CHECKOUT', entity);
    ///start:slot:afterCreateCheckout<<<///end:slot:afterCreateCheckout<<<
  };

  AFTER_REMOVE = () => {
    console.log('AFTER_REMOVE');
    ///start:slot:afterRemove<<<///end:slot:afterRemove<<<
  };

  ON_OPEN_ITEM = item => {
    console.log('ON_OPEN_ITEM', item);

    ///start:slot:onOpenItem<<<
    this.openDialog(item);
    ///end:slot:onOpenItem<<<
  };

  openDialog = item => {
    this.setState({
      catalog: item
    });
  };

  closeDialog = feedback => {
    if (feedback == 'ok') {
      this.refresh();
    }
    this.setState({
      catalog: false
    });
  };
  ///start:slot:js<<<
  componentWillMount() {
    this.initFilterOptions(this.props.router.query.name);
    this.initSortOptions(this.props.router.query.name);
  }
  ///end:slot:js<<<

  render() {
    const { additionalFields, parentType } = this.state;
    return (
      <NoSsr>
        <Container style={{ padding: 20 }} maxWidth='lg'>
          <Typography variant='h4' className='h4' gutterBottom>
            {this.props.router.query.name}
          </Typography>
          <Grid container direction='row'>
            <Grid item xs />
            <Pagination
              activePage={this.state.filterOptions.page}
              itemsCountPerPage={this.state.filterOptions.limit}
              totalItemsCount={this.state.filterOptions.totalItems}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage || 0);
              }}
            />
          </Grid>
          <Paper style={{ overflowX: 'auto', width: '100%', overflowY: 'hidden' }}>
            <Table className='' size='small'>
              <TableHead>
                <TableRow>
                  <TableCell />
                  <TableCell>Value</TableCell>
                  {parentType && <TableCell>Parent</TableCell>}
                  <TableCell>Hidden</TableCell>
                  {additionalFields && additionalFields.map(field => <TableCell key={field.FieldName}>{field.FieldName}</TableCell>)}
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
                      <TableCell>{item.Value}</TableCell>
                      {parentType && <TableCell>{item.ParentValue}</TableCell>}
                      <TableCell>{(item.Hidden || '').toString().toUpperCase()}</TableCell>
                      {additionalFields &&
                        additionalFields.map(field => <TableCell key={field.FieldName}>{item.ConvertedMeta[field.FieldName]}</TableCell>)}
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </Paper>
        </Container>
        <Dialog open={!!this.state.catalog} onClose={this.closeDialog} draggable title='Catalog' okLabel='Save'>
          {dialog => {
            return (
              !this.state.isLoading && (
                <Catalog
                  dialog={dialog}
                  data={this.state.catalog}
                  additionalFields={this.state.additionalFields}
                  parentType={this.state.parentType}
                />
              )
            );
          }}
        </Dialog>
        <AppBar position='fixed' style={{ top: 'auto', bottom: 0 }}>
          <Toolbar variant='dense'>
            <SearchBox bindFilterInput={this.bindFilterInput} value={this.state.filterOptions.filterGeneral} />
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

export default withRouter(CatalogsList);
