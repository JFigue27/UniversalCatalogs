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
import { Button, IconButton } from '@material-ui/core';
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
    let name = this.getParameterByName('Name');
    if (!name) return;
    this.load('Name=' + name);
    ///start:slot:load<<<

    catalogTypeService.GetSingleWhere('Name', name).then(response => {
      this.setState({ additionalFields: response.ConvertedFields, parentType: response.ParentType });
    });
    ///end:slot:load<<<
  }

  AFTER_LOAD = () => {
    console.log('AFTER_LOAD');
    ///start:slot:afterLoad<<<///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<
    let name = this.getParameterByName('Name');
    if (!name) throw 'Error. Name query param is missing.';
    instance.CatalogType = name;
    this.openDialog('catalog', instance);
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
    this.openDialog('catalog', item);
    ///end:slot:onOpenItem<<<
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { additionalFields, parentType, baseList, filterOptions } = this.state;
    return (
      <NoSsr>
        <Button variant='outlined' style={{ margin: 20, width: 220 }} onClick={() => this.navigateTo('/')}>
          <Icon>arrow_back</Icon>
          Catalogs
        </Button>
        <Container style={{ padding: 20 }} maxWidth='lg'>
          <Typography variant='h4' className='h4' gutterBottom>
            {this.props.router.query.name}
          </Typography>
          <Grid container direction='row'>
            <Grid item xs />
            <Pagination
              activePage={filterOptions.page}
              itemsCountPerPage={filterOptions.limit}
              totalItemsCount={filterOptions.itemsCount}
              pageRangeDisplayed={5}
              onChange={newPage => {
                this.pageChanged(newPage);
              }}
            />
          </Grid>
          <Paper style={{ overflowX: 'auto', width: '100%', overflowY: 'hidden' }}>
            <Table className='' size='small'>
              <TableHead>
                <TableRow>
                  <TableCell width={100} />
                  <TableCell>Value</TableCell>
                  {parentType && <TableCell>Parent</TableCell>}
                  <TableCell>Hidden</TableCell>
                  {additionalFields && additionalFields.map(field => <TableCell key={field.FieldName}>{field.FieldName}</TableCell>)}
                </TableRow>
              </TableHead>
              <TableBody>
                {baseList &&
                  baseList.map(item => (
                    <TableRow key={item.Id}>
                      <TableCell>
                        <Grid container direction='row' className='row' justify='center' alignItems='center' spacing={0}>
                          <Grid item xs>
                            <IconButton
                              variant='contained'
                              color='default'
                              onClick={event => {
                                this.openItem(event, item);
                              }}
                              size='small'
                            >
                              <Icon>edit</Icon>
                            </IconButton>
                          </Grid>
                          <Grid item xs>
                            <IconButton
                              variant='contained'
                              color='default'
                              onClick={event => {
                                this.removeItem(event, item);
                              }}
                              size='small'
                            >
                              <Icon>delete</Icon>
                            </IconButton>
                          </Grid>
                        </Grid>
                      </TableCell>
                      <TableCell>{item.Value}</TableCell>
                      {parentType && <TableCell>{item.Parent}</TableCell>}
                      <TableCell>{(item.Hidden || '').toString()}</TableCell>
                      {additionalFields &&
                        additionalFields.map(field => (
                          <TableCell key={field.FieldName}>
                            {(item.ConvertedMeta[field.FieldName] && item.ConvertedMeta[field.FieldName].toString()) || ''}
                          </TableCell>
                        ))}
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </Paper>
        </Container>
        {/* <pre>{JSON.stringify(baseList, null, 3)}</pre> */}
        <Dialog opener={this} id='catalog' title='Catalog' okLabel='Save'>
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
        <AppBar position='fixed' style={{ top: 'auto', bottom: 0, backgroundColor: '#333333' }}>
          <Toolbar variant='dense'>
            <SearchBox
              bindFilterInput={this.bindFilterInput}
              value={filterOptions.filterGeneral}
              clear={() => this.clearInput('filterGeneral')}
            />
            <Grid item xs />
            <Button
              variant='contained'
              color='default'
              className=''
              onClick={event => {
                this.createInstance({});
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
