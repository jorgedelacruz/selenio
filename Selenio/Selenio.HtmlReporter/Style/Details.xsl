<?xml version="1.0" encoding="iso-8859-1"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <link type="text/css" rel="stylesheet" href="Template\Style.css"/>
      </head>
      <body>
        <div class="heading-main">
          <a id="gotobottom" href="#gototop">GO TO BOTTOM &#9660;</a>
          <div id="test_case_summary">
            <div class="testrundetails">
              <b>Test name: </b>
              <label>
                <xsl:value-of select="testdetails/test/@name"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>Test description: </b>
              <label>
                <xsl:value-of select="testdetails/test/description"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>OS version: </b>
              <label>
                <xsl:value-of select="testdetails/test/@osversion"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>Browser name: </b>
              <label>
                <xsl:value-of select="testdetails/test/@browsername"/>
              </label>
            </div>
            <div class="testrundetails">
              <b>Run type: </b>
              <label>
                <xsl:value-of select="testdetails/test/@runtype"/>
              </label>
            </div>
          </div>
        </div>

        <div id="test_step_summary_container">
          <b>
            Test Steps
          </b>
          <div>
            <xsl:for-each select="testdetails/test/teststep">
              <xsl:variable name="StepNumber">
                <xsl:value-of select="@stepnumber" />
              </xsl:variable>
              <div class="teststep">
                <a href="#teststep_{$StepNumber}">
                  <xsl:value-of select="@description"/>
                </a>
              </div>
            </xsl:for-each>
          </div>
        </div>

        <div id="test_step_detail">
          <!--<div class="heading-section">Detailed Steps</div>-->
          <table class="summary">
            <tr class="heading">
              <th class="heading">Element</th>
              <th class="heading">Type</th>
              <th class="heading">Action</th>
              <th class="heading">Property</th>
              <th class="heading">Exp. Value</th>
              <th class="heading">Act. Value</th>
              <th class="heading">Result</th>
              <th class="heading">Error</th>
              <th class="heading">Time</th>
              <th class="heading">Screenshot</th>
            </tr>
            <xsl:for-each select="testdetails/test/teststep">
              <xsl:variable name="StepNumber">
                <xsl:value-of select="@stepnumber" />
              </xsl:variable>
              <td colspan="10" id="teststep_{$StepNumber}" class="teststep">
                <xsl:value-of select="@description"/>
                <a style="float: right" href="#gotobottom">GO TO TOP &#9650;</a>
              </td>
              <xsl:for-each select="dbg">
                <tr>
                  <xsl:variable name="TestLink">
                    <xsl:value-of select="@Screenshot" />
                  </xsl:variable>
                  <xsl:choose>
                    <xsl:when test="@Method = 'SetParent'">
                      <xsl:if test="@Result = 'Passed'">
                        <td class="valuepassheader">
                          <xsl:value-of select="@Control"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@ControlType"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@Method"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@Property"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@Value"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@AValue"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@Result"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@Error"/>
                        </td>
                        <td class="valuepassheader">
                          <xsl:value-of select="@stepTime"/>
                        </td>
                        <xsl:if test="@Screenshot != ''">
                          <td class="valuepassheader">
                            <a href="{$TestLink}">Screenshot</a>
                          </td>
                        </xsl:if>
                        <xsl:if test="@Screenshot = ''">
                          <td class="valuepassheader"></td>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="@Result = 'Failed'">
                        <td class="valuefailheader">
                          <xsl:value-of select="@Control"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@ControlType"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@Method"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@Property"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@Value"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@AValue"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@Result"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@Error"/>
                        </td>
                        <td class="valuefailheader">
                          <xsl:value-of select="@stepTime"/>
                        </td>
                        <xsl:if test="@Screenshot != ''">
                          <td class="valuefailheader">
                            <a href="{$TestLink}">Screenshot</a>
                          </td>
                        </xsl:if>
                        <xsl:if test="@Screenshot = ''">
                          <td class="valuefailheader"></td>
                        </xsl:if>
                      </xsl:if>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:if test="@Result = 'Passed'">
                        <td class="valuepass">
                          <xsl:value-of select="@Control"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@ControlType"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@Method"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@Property"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@Value"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@AValue"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@Result"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@Error"/>
                        </td>
                        <td class="valuepass">
                          <xsl:value-of select="@stepTime"/>
                        </td>
                        <xsl:if test="@Screenshot != ''">
                          <td class="valuepass">
                            <a href="{$TestLink}">Screenshot</a>
                          </td>
                        </xsl:if>
                        <xsl:if test="@Screenshot = ''">
                          <td class="valuepass"></td>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="@Result = 'Failed'">
                        <td class="valuefail">
                          <xsl:value-of select="@Control"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@ControlType"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@Method"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@Property"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@Value"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@AValue"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@Result"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@Error"/>
                        </td>
                        <td class="valuefail">
                          <xsl:value-of select="@stepTime"/>
                        </td>
                        <xsl:if test="@Screenshot != ''">
                          <td class="valuefail">
                            <a href="{$TestLink}">Screenshot</a>
                          </td>
                        </xsl:if>
                        <xsl:if test="@Screenshot = ''">
                          <td class="valuefail"></td>
                        </xsl:if>
                      </xsl:if>
                    </xsl:otherwise>
                  </xsl:choose>
                </tr>
              </xsl:for-each>
            </xsl:for-each>
          </table>
        </div>

        <div class="heading-section">
          <a style="float: right" id="gototop" href="#gotobottom">GO TO TOP &#9650;</a>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>