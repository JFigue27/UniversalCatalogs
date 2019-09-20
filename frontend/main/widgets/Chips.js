import React from 'react';
import { Paper, Chip, NoSsr, Icon } from '@material-ui/core';
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
    const { keyProp = 'Id', labelProp = 'Value' } = this.props;
    this.onChange = onChange;

    return (
      <>
        <NoSsr>
          {/* <pre>{JSON.stringify(selected, null, 3)}</pre> */}
          <Paper style={{ minHeight: 32, marginTop: 0 }} className='Chips' elevation={0}>
            {selected.map((item, index) => {
              return (
                <Chip
                  size='small'
                  key={index}
                  color='default'
                  variant='default'
                  label={item.label}
                  className={`Person-Chip ${item.classes || ''}`}
                  onDelete={() => this.onRemove(index)}
                  icon={item.icon && <Icon style={{ color: 'inherit', fontSize: '1.2em' }}>{item.icon}</Icon>}
                />
              );
            })}
            <Select
              options={(allOptions || []).filter(opt => !selected.some(s => s.value == opt.value))}
              onChange={this.handleSelectChange}
              placement={placement || 'top'}
              label={''}
              value={this.state.selectValue || ''}
              style={{ width: 200, maxWidth: 200, display: 'inline-block' }}
              keyProp={keyProp}
              labelProp={labelProp}
            />
          </Paper>
        </NoSsr>
      </>
    );
  }
}
