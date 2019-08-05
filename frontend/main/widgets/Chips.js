import React from 'react';
import { Paper, Chip, NoSsr } from '@material-ui/core';
import Select from './Select';

export default class Chips extends React.Component {
  state = {
    selected: [],
    selectValue: ''
  };

  adapterIn = value => {
    let items = typeof value == 'string' ? JSON.parse(value || '[]') : value || [];
    if (items.length) {
      const { keyProp = 'Id', labelProp = 'Value' } = this.props;
      return items.map(item => {
        return {
          ...item,
          value: item[keyProp],
          label: item[labelProp]
        };
      });
    } else {
      return [];
    }
  };

  adapterOut = selected => {
    const { keyProp = 'Id', labelProp = 'Value', json } = this.props;

    let result = selected.map(item => {
      let adapted = { ...item };
      adapted[keyProp] = item.value;
      adapted[labelProp] = item.label;
      return adapted;
    });

    if (json) return JSON.stringify(result);

    return result;
  };

  componentDidMount() {
    this.setState({
      selected: this.adapterIn(this.props.value),
      allOptions: this.adapterIn(this.props.options)
    });
  }

  componentDidUpdate(prevProps) {
    const { options: prevOptions, value: prevValue } = prevProps;
    const { options, value } = this.props;

    if (!prevOptions && options) {
      this.setState({
        allOptions: this.adapterIn(options)
      });
    }

    if ((!prevValue && value) || prevValue != value) {
      this.setState({
        selected: this.adapterIn(value)
      });
    }
  }

  handleSelectChange = item => {
    if (item == null) return;

    let { selected } = this.state;
    selected.push(item);
    this.setState({ selected, selectValue: '' });

    this.onChange(this.adapterOut(selected));
  };

  onRemove = index => {
    let { selected } = this.state;
    selected.splice(index, 1);
    this.setState({ selected });
    this.onChange(this.adapterOut(selected));
  };

  render() {
    const { onChange, placeholder, placement } = this.props;
    const { selected, allOptions } = this.state;
    this.onChange = onChange;

    return (
      <>
        <NoSsr>
          <Paper style={{ minHeight: 32, marginTop: 20 }} className='Chips' elevation={0}>
            {selected.map((item, index) => {
              return (
                <Chip
                  key={item.value}
                  color='primary'
                  variant='outlined'
                  label={item.label}
                  className='Person-Chip'
                  onDelete={() => this.onRemove(index)}
                />
              );
            })}
            <Select
              options={(allOptions || []).filter(opt => !selected.some(s => s.value == opt.value))}
              onChange={this.handleSelectChange}
              placement={placement || 'top'}
              label={placeholder || ''}
              value={this.state.selectValue || ''}
              style={{ padding: 5, marginBottom: 3 }}
            />
          </Paper>
        </NoSsr>
      </>
    );
  }
}
