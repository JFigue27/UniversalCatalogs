import moment from 'moment';
import DialogActions from '@material-ui/core/DialogActions';
import { Grid, Button, Typography, TextField } from '@material-ui/core';
import ItemFormContainer from './item.form.container';

///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {};

class Item extends ItemFormContainer {
  constructor(props) {
    super(props, config);
  }

  componentDidMount() {
    this.load(this.props.entity);
  }

  AFTER_SAVE = () => {
    const { dialog } = this.props;
    if (dialog) dialog.close();
  };

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    const { dialog } = this.props;
    if (dialog) dialog.onOk = this.onDialogOk;

    return (
      <>
        <pre>{JSON.stringify(this.state.baseEntity, null, 3)}</pre>
        <Grid className='' container direction='column' item xs={12}>
          <Typography variant='h3' className='' gutterBottom>
            Item
          </Typography>
          <TextField
            label='Item Number'
            value={this.state.baseEntity.ItemNumber || ''}
            onChange={event => this.handleInputChange(event, 'ItemNumber')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
          />
          <TextField
            label='Item Description'
            value={this.state.baseEntity.ItemDescription || ''}
            onChange={event => this.handleInputChange(event, 'ItemDescription')}
            style={{ textAlign: 'left' }}
            margin='normal'
            disabled={this.isDisabled}
          />
        </Grid>
      </>
    );
  }
}

export default Item;
