import React, { Component } from 'react';
import Graphs from './Graphs';

export class Weather extends Component {
  static displayName = Weather.name;

  render() {
      let contents = Weather.renderForecastsTable()
      return (
          <div>
              <h1 id="tabelLabel" >Weather</h1>
              {contents}
          </div>
      );
    }

    static renderForecastsTable() {
        return (
                <Graphs />
        );
    }
}
