function lineGraph(el) {
    
    var viewWidth = $(el).width();
    var viewHeight = $(el).height();

    var resultSet = {
        rows:
        {
            1359311711
        },
        columns:
        {
            "type": "Date",
            
        }
    };

    // Margin conventions
    var margin = { top: 20, right: 20, bottom: 30, left: 50 };
    var width = viewWidth - margin.left - margin.right;
    var height = viewHeight - margin.top - margin.bottom;

    var xDate = resultSet.columns[0]["type"] == 'Date';

    // Convert epoch times to dates if necessary
    var data = resultSet.rows.map(function (d) {
        return {
            x: xDate ? new Date(d[0]) : d[0],
            y: d[1]
        };
    });

    var xScale = xDate ? d3.time.scale() : d3.scale.linear();
    var yScale = d3.scale.linear();

    var x = xScale.range([0, width]);
    var y = yScale.range([height, 0]);

    var xAxis = d3.svg.axis().scale(x).orient("bottom");
    var yAxis = d3.svg.axis().scale(y).orient("left");

    var line = d3.svg.line()
        .x(function (d) { return x(d.x); })
        .y(function (d) { return y(d.y); });

    // This should be a transition
    d3.select("svg").remove();

    // Drop the SVG canvas into the container
    var graph = d3.select("#graph")
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    // Clamp the axes to min and max values
    var xExtent = d3.extent(data, function (d) { return d.x; });
    var yExtent = d3.extent(data, function (d) { return d.y; });
    x.domain(xExtent);
    y.domain(yExtent);

    graph.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis);

    // Draw the y axis label in the top right offset of itself
    var yLabel = resultSet.columns[1].name;
    graph.append("g")
        .attr("class", "y axis")
        .call(yAxis)
        .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .text(yLabel);

    // Append the graph line (could be multiple lines)
    graph.append("path")
        .datum(data)
        .attr("class", "line")
        .attr("d", line);
}