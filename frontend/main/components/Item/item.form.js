import moment from 'moment';
import DialogActions from '@material-ui/core/DialogActions';
import Button from '@material-ui/core/Button';
import ItemFormContainer from './item.form.container';

///start:slot:dependencies<<<///end:slot:dependencies<<<

const config = {};

class Item extends ItemFormContainer {
  constructor(props) {
    super(props, config);
  }

  componentDidMount() {
    console.log('component did mount');
    this.load(this.props.entity);
  }

  ///start:slot:js<<<///end:slot:js<<<

  render() {
    return (
      <>
        <pre>{JSON.stringify(this.state.baseEntity, null, 3)}</pre>
      </>
    );
  }
}

export default Item;
