import React from 'react';
import { withRouter } from 'next/router';
import { NoSsr, Typography, Grid } from '@material-ui/core';
import SearchBox from '../../widgets/Searchbox';
import Pagination from 'react-js-pagination';
import ListContainer from '../../core/ListContainer';
import { Table } from '@material-ui/core';
import { TableHead } from '@material-ui/core';
import { TableBody } from '@material-ui/core';
import { TableRow } from '@material-ui/core';
import { TableCell } from '@material-ui/core';
import { Button } from '@material-ui/core';
import { Icon } from '@material-ui/core';
import { InputBase } from '@material-ui/core';

///start:slot:dependencies<<<///end:slot:dependencies<<<

const service = {};
const defaultConfig = {
  service,
  ///start:slot:config<<<
  autoAdd: true
  ///end:slot:config<<<
};

class AdditionalFieldsList extends ListContainer {
  constructor(props, config) {
    Object.assign(defaultConfig, config);
    super(props, defaultConfig);
    this.tableRef = React.createRef();
  }

  componentDidMount() {
    console.log('List did mount');
    let baseList = this.props.parent.ConvertedFields || [];
    baseList.push({});
    this.setState({
      baseList
    });

    ///start:slot:didMount<<<
    this.AFTER_LOAD();
    ///end:slot:didMount<<<
  }

  AFTER_LOAD = () => {
    console.log('AFTER_LOAD');
    ///start:slot:afterLoad<<<
    this.enableCellNavigation(this.tableRef.current);
    ///end:slot:afterLoad<<<
  };

  AFTER_CREATE = instance => {
    console.log('AFTER_CREATE', instance);

    ///start:slot:afterCreate<<<///end:slot:afterCreate<<<
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

    ///start:slot:onOpenItem<<<///end:slot:onOpenItem<<<
  };

  ///start:slot:js<<<
  ON_CHANGE = data => {
    this.props.onChange(data);
  };

  componentDidUpdate() {
    this.enableCellNavigation(this.tableRef.current);
  }
  ///end:slot:js<<<

  render() {
    return (
      <NoSsr>
        <Table className='' size='small' ref={this.tableRef}>
          <TableHead>
            <TableRow>
              <TableCell>Field Name </TableCell>
              <TableCell>Field Type </TableCell>
              <TableCell />
            </TableRow>
          </TableHead>
          <TableBody>
            {this.state.baseList &&
              this.state.baseList.map((item, index) => (
                <TableRow key={index}>
                  <TableCell>
                    <InputBase
                      type='text'
                      className='filled'
                      autoComplete='off'
                      disabled={this.isDisabled}
                      readOnly={false}
                      onChange={event => this.handleInputChange(event, 'FieldName', index)}
                      value={item.FieldName || ''}
                      fullWidth
                    />
                  </TableCell>
                  <TableCell>
                    <InputBase
                      type='text'
                      className='filled'
                      autoComplete='off'
                      disabled={this.isDisabled}
                      readOnly={false}
                      onChange={event => this.handleInputChange(event, 'FieldType', index)}
                      value={item.FieldType || ''}
                      fullWidth
                    />
                  </TableCell>
                  <TableCell>
                    <Grid container direction='row' className='row' justify='center' alignItems='center' spacing={2}>
                      <Grid item xs>
                        <Button
                          variant='contained'
                          color='default'
                          className='md-warn md-hue-1'
                          onClick={event => {
                            this.localRemoveItem(event, index);
                          }}
                          size='small'
                        >
                          <Icon>delete</Icon>
                        </Button>
                      </Grid>
                    </Grid>
                  </TableCell>
                </TableRow>
              ))}
          </TableBody>
        </Table>
      </NoSsr>
    );
  }
}

export default withRouter(AdditionalFieldsList);
